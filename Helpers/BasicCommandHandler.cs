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
using WBot2.Data;
using WBot2.Helpers.Interfaces;

namespace WBot2.Helpers
{
    public class BasicCommandHandler : ICommandHandler
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<BasicCommandHandler> _logger;
        protected readonly DiscordOptions _baseOptions;
        protected readonly DiscordClient _discordClient;
        protected readonly IConverterHelper _converterHelper;

        public List<CommandModule> CommandModules { get; }
        public List<Command> Commands { get; }
        public BasicCommandHandler(IServiceProvider serviceProvider, ILogger<BasicCommandHandler> logger, DiscordClient discordClient, IConverterHelper converterHelper)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            _discordClient = discordClient;
            _converterHelper = converterHelper;

            CommandModules = StaticHelpers.GetModules<CommandModule>( new object[] { _serviceProvider, this });
            Commands = new();

            foreach (CommandModule module in CommandModules)
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
                if (cmdp[i].IsOptional && args.Count() <= i)
                {
                    convertTasks.Add(Task.FromResult(cmdp[i].DefaultValue));
                    continue;
                }
                if (type == args[i].GetType())
                {
                    //builtArgs.Add(args[i]);
                    convertTasks.Add(Task.FromResult<object>(args[i]));
                    continue;
                } 
                else if (cmdp[i].GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0)
                {
                    /*Type elementT = type.GetElementType();
                    var remaining = args.Skip(i).ToList();
                    if (elementT == typeof(string))
                    {
                        builtArgs.Add(remaining.ToArray());
                        break;
                    }
                    List<object> converts = new();
                    foreach (string arg in remaining)
                    {
                        var converted = await _converterHelper.ConvertParameterAsync(ctx, elementT, arg, i);
                        converts.Add(converted);
                    }
                    await ctx.RespondAsync($"PARAMS DEBUG!\nCurrent arg: {i}\nTotal args: {args.Count}\nPost-Skip args: {remaining.Count}");
                    break;*/
                    throw new NotImplementedException("Params arrays are not yet implemented");
                }
                try
                {
                    var task = _converterHelper.ConvertParameterAsync(ctx, type, args[i], i);
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

#nullable enable
        private Command? FindCommand(string cmd)
        {
            Command? ret = Commands.FirstOrDefault(x =>
            {
                bool fuck = x.Aliases == null;
                if (x.Aliases != null)
                    return x.Aliases?.Any(x => x == cmd)??false;
                return x.Name == cmd;
            });

            return ret ?? null;
        }

        private async Task RunCommandAsync(string cmdn, MessageCreateEventArgs e, List<string> args)
        {
            Command? cmd = FindCommand(cmdn);
            if (cmd == null)
            {
                await e.Message.RespondAsync($"Unknown command, type `{_baseOptions.CommandPrefix} help` for all commands");
                return;
            }
            Command command = cmd;

            CommandContext ctx = new CommandContext()
            {
                Client = this._discordClient,
                Message = e.Message,
                RawArguments = args,
                Command = command
            };
#nullable disable
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
                if (await attribute.CheckAttribute(ctx, _serviceProvider) != false)
                    continue;
                else
                {
                    await e.Message.RespondAsync($"Command failed: {attribute.FailString}");
                    return;
                }
            }

            string suppliedargs = "";
            foreach (object obj in builtArgs.Skip(1))
            {
                suppliedargs += $"{obj.GetType()}({obj})\n";
            }

            _logger.LogInformation($"Command {command.Name} with args {string.Join(" ", args)} run by {e.Author.Username}");
            _logger.LogInformation($"Supplied built parameters were\n{suppliedargs}");
            await command.Call(CommandModules.FirstOrDefault(x => x.GetType().FullName == command.Method.DeclaringType.FullName), builtArgs);
        }

        public async Task ProcessCommand(DiscordClient sender, MessageCreateEventArgs e)
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
