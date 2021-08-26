using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WBot2.Commands;
using WBot2.Helpers.Interfaces;

namespace WBot2.Helpers
{
    public class BasicHelpFormatter : IHelpFormatter<DiscordEmbedBuilder>
    {
        public async Task<DiscordEmbedBuilder> FormatHelp(ICommandHandler commandHandler)
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Commands")
                .WithDescription("This is a list of available commands, sorted into their modules.");

            foreach (CommandModule baseModule in commandHandler.CommandModules)
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