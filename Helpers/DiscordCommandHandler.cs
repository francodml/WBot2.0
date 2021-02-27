using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WBot2.Data;
using System.Reflection;
using System.Linq;
using WBot2.Commands;
using WBot2.Commands.Attributes;
using Microsoft.Extensions.Logging;

namespace WBot2.Helpers
{
    public class DiscordCommandHandler : ICommandHandler
    {
        protected readonly IServiceProvider _serviceProvider;
        public ILogger<DiscordCommandHandler> _logger;
        protected readonly DiscordOptions _baseOptions;

        private List<BaseCommandModule> _commandModules;
        private List<MethodInfo> _commands;
        public DiscordCommandHandler(IServiceProvider serviceProvider, ILogger<DiscordCommandHandler> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;

            _commandModules = new();
            _commands = new();
            foreach(Type type in Assembly.GetAssembly(typeof(BaseCommandModule)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseCommandModule))))
            {
                _commandModules.Add((BaseCommandModule)Activator.CreateInstance(type, new object[] { _serviceProvider }));
            }

            foreach (BaseCommandModule module in _commandModules)
            {
                var cmds = module.GetType().GetMethods().Where(x => x.GetCustomAttribute<CommandAttribute>() != null);
                foreach (MethodInfo cmd in cmds)
                {
                    _commands.Add(cmd);
                }
            }
        }

        private Task<MethodInfo> FindCommand(string cmd)
        {
            return Task.Run(() => _commands.FirstOrDefault(x => x.GetCustomAttribute<CommandAttribute>().Name == cmd));
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
            /*if (string.IsNullOrEmpty(cmd) || cmd == "help")
            {
                await e.Message.RespondAsync($"Commands: {string.Join(", ", _commands.Where(x => Assembly.GetAssembly(nameof(x)).GetCustomAttribute(CommandAttribute).Select(x => x.Command))}");
                return;
            }*/
            args = args.Skip(1).ToList();
            var command = await FindCommand(cmd);
            if (command == null)
            {
                await e.Message.RespondAsync($"Unknown command, type `{_baseOptions.CommandPrefix} help` for all commands");
                return;
            }

            _logger.LogInformation($"Command {command} with args {string.Join(" ", args)} run by {e.Author.Username}");
            try
            {
                await Task.Run( () => command.Invoke(_commandModules.FirstOrDefault(x => x.GetType().FullName == command.DeclaringType.FullName), new object[] { e }));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, $"Failed to run {command} with args {string.Join(" ", args)} run by {e.Author.Username}");
                await e.Message.RespondAsync($"Failed to run command: {ex.Message}");
            }
        }
    }
}
