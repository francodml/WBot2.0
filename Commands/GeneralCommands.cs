using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;
using WBot2.Helpers.Interfaces;

namespace WBot2.Commands
{
    public class GeneralCommands : BaseCommandModule
    {
        protected readonly ILogger _logger;
        protected readonly IHelpFormatter<DiscordEmbedBuilder> _helpFormatter;

        public GeneralCommands(IServiceProvider serviceProvider, ICommandHandler commandHandler) : base(serviceProvider, commandHandler)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<GeneralCommands>>();
            _helpFormatter = serviceProvider.GetRequiredService<IHelpFormatter<DiscordEmbedBuilder>>();
        }

        [Command("help"), Description("Shows all commands and their descriptions")]
        public async Task Help(MessageCreateEventArgs e, List<string> args)
        {
            DiscordEmbedBuilder embed = await _helpFormatter.FormatHelp(RegisteringHandler);
            embed.WithAuthor(_discordClient.CurrentUser.Username, iconUrl:_discordClient.CurrentUser.AvatarUrl);
            await e.Message.RespondAsync($"{e.Author.Mention} these are the available commands",embed);
        }

        [Command("ping")]
        [Alias("pong", "p")]
        public async Task Ping(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"{e.Author.Mention} Pong!");
        }
        [Command("say")]
        public async Task Say(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"You told me to say: '{string.Join(" ", args)}'");
        }

        [Command("uwu")]
        [NeedsPermissions(DSharpPlus.Permissions.Administrator | DSharpPlus.Permissions.BanMembers)]
        public async Task Uwu(MessageCreateEventArgs e, List<string> args)
        {
            await Task.Delay(10000);
            await e.Message.RespondAsync("UwU");
        }
    }
}
