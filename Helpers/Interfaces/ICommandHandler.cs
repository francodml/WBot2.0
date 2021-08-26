using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Helpers.Interfaces
{
    public interface ICommandHandler
    {
        Task ProcessCommand(DiscordClient sender, MessageCreateEventArgs e);

        public List<Command> Commands { get; }

        public List<CommandModule> CommandModules { get; }
    }
}
