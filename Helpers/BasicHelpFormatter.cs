using DSharpPlus.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WBot2.Helpers.Interfaces;
using WBot2.Commands;

namespace WBot2.Helpers
{
    public class BasicHelpFormatter : IHelpFormatter<DiscordEmbedBuilder>
    {
        public async Task<DiscordEmbedBuilder> FormatHelp(ICommandHandler commandHandler)
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Commands")
                .WithDescription("This is a list of available commands, sorted into their modules.");

            foreach (BaseCommandModule baseModule in commandHandler.CommandModules)
            {
                var cmds = commandHandler.Commands.Where(x => x.Module == baseModule);
                List<string> cmdDescs = cmds.Select(x => $"{x.Name}: {x.Description}\n").ToList();
                string listString = cmdDescs.Aggregate((a, b) => a + b);
                embed.AddField(baseModule.ModuleName, listString);
            }
            return embed;
        }
    }
}