using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Helpers.Interfaces
{
    public interface ICommandHandler
    {
        Task ProcessCommands(DiscordClient sender, MessageCreateEventArgs e);

        public List<Command> Commands { get; }

        public List<BaseCommandModule> CommandModules { get; }
    }
}
