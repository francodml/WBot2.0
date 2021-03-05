using DSharpPlus.Entities;
using System.Threading.Tasks;
using WBot2.Helpers.Interfaces;

namespace WBot2.Helpers
{
    public class BasicHelpFormatter : IHelpFormatter
    {
        public Task<DiscordEmbed> FormatHelp(ICommandHandler commandHandler)
        {
            throw new System.NotImplementedException();
        }
    }
}