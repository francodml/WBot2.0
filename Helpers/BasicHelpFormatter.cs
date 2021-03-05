using DSharpPlus.Entities;
using System.Threading.Tasks;
using WBot2.Helpers.Interfaces;
using WBot2.Commands;

namespace WBot2.Helpers
{
    public class BasicHelpFormatter : IHelpFormatter<DiscordEmbedBuilder>
    {
        public async Task<DiscordEmbedBuilder> FormatHelp(ICommandHandler commandHandler)
        {
            var embed = new DiscordEmbedBuilder()
                .WithTitle("Comandos")
                .WithDescription("Esta es la lista de comandos disponibles");
            foreach (Command command in commandHandler.Commands)
            {
                embed.AddField(command.Name, command.Description);
            }
            //await e.Message.RespondAsync($"Commands: {string.Join(", ", Commands.Select(x => x.GetCustomAttribute<CommandAttribute>().Name))}");
            return embed;
        }
    }
}