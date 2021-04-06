using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WBot2.Commands;
using WBot2.Commands.Attributes;
using WBot2.Helpers.Interfaces;
using WBot2.Data;
using WBot2.Extensions;

namespace WBot2.Helpers
{
    public class BasicCommandHandler : ICommandHandler
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<BasicCommandHandler> _logger;
        protected readonly DiscordOptions _baseOptions;
        protected readonly DiscordClient _discordClient;
        protected readonly IConverterHelper _converterHelper;

        public List<BaseCommandModule> CommandModules { get; }
        public List<Command> Commands { get; }
        public BasicCommandHandler(IServiceProvider serviceProvider, ILogger<BasicCommandHandler> logger, DiscordClient discordClient, IConverterHelper converterHelper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            _discordClient = discordClient;
            _converterHelper = converterHelper;

            CommandModules = StaticHelpers.GetModules<BaseCommandModule>( new object[] { _serviceProvider, this });
            Commands = new();

            foreach (BaseCommandModule module in CommandModules)
            {
                var infos = module.GetType().GetMethods().Where(x => x.GetCustomAttribute<CommandAttribute>() != null);
                foreach (MethodInfo info in infos)
                {
                    Command cmd = new()
                    {
                        Method = info,
                        Module = module
                    };
                    Commands.Add(cmd);
                }
            }
        }

        private async Task<object[]> BuildArguments(CommandContext ctx, List<string> args)
        {
            var cmdp = ctx.Command.Parameters.Skip(1).ToArray();
            List<object> builtArgs = new();
            List<Task<object>> convertTasks = new();
            builtArgs.Add(ctx);
            for (int i = 0; i < cmdp.Count(); i++)
            {
                var type = cmdp[i].ParameterType;
                if (type == args[i].GetType())
                {
                    builtArgs.Add(args[i]);
                    continue;
                }
                try
                {
                    MethodInfo generic = _converterHelper.GetType().GetMethod("ConvertParameterAsync").MakeGenericMethod(type);
                    var task = (Task<object>)generic.Invoke(_converterHelper, new object[] { ctx, args[i], i });
                    convertTasks.Add(task);
                } 
                catch (Exception e)
                {
                    switch (e)
                    {
                        case NullReferenceException nullReference:
                            break;
                        default:
                            throw;
                    }
                }
            }
            object[] result = await Task.WhenAll(convertTasks);
            result.ToList().ForEach(x => builtArgs.Add(x));
            return builtArgs.ToArray();
        }

        private Command FindCommand(string cmd)
        {
            //TODO: implement command overloading (optional args)
            return Commands.FirstOrDefault(x =>
            {
                AliasAttribute attr = x.GetCustomAttribute<AliasAttribute>();
                bool aliastest = false;
                if (attr != null)
                {
                    aliastest = attr.Aliases.Any(x => x == cmd);
                }
                return x.GetCustomAttribute<CommandAttribute>().Name == cmd || aliastest;
            });
        }

        private async Task RunCommandAsync(string cmdn, MessageCreateEventArgs e, List<string> args)
        {
            Command? cmd = FindCommand(cmdn);
            if (cmd == null)
            {
                await e.Message.RespondAsync($"Unknown command, type `{_baseOptions.CommandPrefix} help` for all commands");
                return;
            }
            Command command = cmd.GetValueOrDefault();

            CommandContext ctx = new CommandContext()
            {
                Client = this._discordClient,
                Message = e.Message,
                RawArguments = args,
                Command = command
            };

            object[] builtArgs;
            try
            {
                builtArgs = await BuildArguments(ctx, args);
            }
            catch
            {
                throw;
            }

            var checkAttribs = command.GetCustomAttributes().Where(x => x.GetType().IsAssignableTo(typeof(ICheckAttribute)));

            foreach (ICheckAttribute attribute in checkAttribs)
            {
                if (await attribute.CheckAttribute(e, _serviceProvider) != false)
                    continue;
                else
                {
                    await e.Message.RespondAsync($"Command failed: {attribute.FailString}");
                    return;
                }
            }

            _logger.LogInformation($"Command {command.Name} with args {string.Join(" ", args)} run by {e.Author.Username}");
            await command.Call(CommandModules.FirstOrDefault(x => x.GetType().FullName == command.Method.DeclaringType.FullName), builtArgs);
        }
        public async Task ProcessCommands(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (!e.Message.Content.ToLower().StartsWith(_baseOptions.CommandPrefix)) { return; }
            var argString = e.Message.Content.Trim().Substring(_baseOptions.CommandPrefix.Length).Trim();
            var args = argString.Split('"')
                     .Select((element, index) => index % 2 == 0  // If even index
                                           ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                                           : new string[] { element })  // Keep the entire item
                     .SelectMany(element => element).ToList();
            var cmd = args.FirstOrDefault();
            if (string.IsNullOrEmpty(cmd))
                return;
            args = args.Skip(1).ToList();

            try
            {
                await e.Channel.TriggerTypingAsync();
                await RunCommandAsync(cmd, e, args);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Failed to run {FindCommand(cmd)} with args {string.Join(" ", args)} run by {e.Author.Username}");
                await e.Message.RespondAsync($"Failed to run command: {ex.Message}");
            }
        }
    }
}
