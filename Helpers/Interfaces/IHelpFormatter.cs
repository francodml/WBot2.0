using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;

namespace WBot2.Helpers.Interfaces
{
    public interface IHelpFormatter
    {
        public abstract Task<DiscordEmbed> FormatHelp(ICommandHandler commandHandler);
    }
}