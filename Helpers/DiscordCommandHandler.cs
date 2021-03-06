﻿using DSharpPlus;
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
using WBot2.Extensions;

namespace WBot2.Helpers
{
    public class DiscordCommandHandler : ICommandHandler
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger<DiscordCommandHandler> _logger;
        protected readonly DiscordOptions _baseOptions;
        protected readonly DiscordClient _discordClient;

        private List<BaseCommandModule> _commandModules;
        private List<MethodInfo> _commands;
        public DiscordCommandHandler(IServiceProvider serviceProvider, ILogger<DiscordCommandHandler> logger, DiscordClient discordClient)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            _discordClient = discordClient;

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

        private MethodInfo FindCommand(string cmd)
        {
            //TODO: implement command overloading (optional args)
            return _commands.FirstOrDefault(x =>
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

        private async Task RunCommandAsync(string cmd, MessageCreateEventArgs e, List<string> args)
        {
            MethodInfo command = FindCommand(cmd);
            if (command == null)
            {
                await e.Message.RespondAsync($"Unknown command, type `{_baseOptions.CommandPrefix} help` for all commands");
                return;
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

            _logger.LogInformation($"Command {command} with args {string.Join(" ", args)} run by {e.Author.Username}");
            await (Task)command.Invoke(_commandModules.FirstOrDefault(x => x.GetType().FullName == command.DeclaringType.FullName), new object[] { e, args });
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
            if (string.IsNullOrEmpty(cmd) || cmd == "help")
            {
                //TODO: Move this to a dedicated "help" command, leave this as fallback if one isn't defined. Add command descriptions
                await e.Message.RespondAsync($"Commands: {string.Join(", ", _commands.Select(x => x.GetCustomAttribute<CommandAttribute>().Name))}");
                return;
            }
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
