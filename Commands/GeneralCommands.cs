using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;
using WBot2.Helpers.Interfaces;

namespace WBot2.Commands
{
    public class GeneralCommands : CommandModule
    {
        public override string ModuleName => "General Commands";
        protected readonly ILogger _logger;
        protected readonly IHelpFormatter<DiscordEmbedBuilder> _helpFormatter;

        public GeneralCommands(IServiceProvider serviceProvider, ICommandHandler commandHandler) : base(serviceProvider, commandHandler)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<GeneralCommands>>();
            _helpFormatter = serviceProvider.GetRequiredService<IHelpFormatter<DiscordEmbedBuilder>>();
        }


        [Command("help"), Description("Shows all commands and their descriptions")]
        public async Task Help(CommandContext ctx)
        {
            DiscordEmbedBuilder embed = await _helpFormatter.FormatHelp(RegisteringHandler);
            embed.WithAuthor(_discordClient.CurrentUser.Username, iconUrl: _discordClient.CurrentUser.AvatarUrl)
                .WithColor(ctx.Member.Color);
            await ctx.Message.RespondAsync($"{ctx.User.Mention} these are the available commands", embed);
        }

        [Command("ping"), Description("Pings the bot, and gets a reply!")]
        [Alias("pong", "p")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Message.RespondAsync($"{ctx.User.Mention} Pong!");
        }
        [Command("say"), Description("Makes the bot say something")]
        public async Task Say(CommandContext ctx, string args)
        {
            await ctx.Message.RespondAsync($"You told me to say: '{args}'");
        }

        [Command("uwu")]
        [NeedsPermissions(DSharpPlus.Permissions.Administrator | DSharpPlus.Permissions.BanMembers)]
        public async Task Uwu(CommandContext ctx, int waitTime = 10, string msg = "UwU")
        {
            await ctx.RespondAsync($"Ok, I'll wait {waitTime} seconds and say that!");
            waitTime *= 1000;
            await Task.Delay(waitTime);
            await ctx.TriggerTypingAsync();
            await ctx.Message.RespondAsync($"{msg}");
        }

        [Command("paramstest"), Alias("ptest", "pt")]
        public async Task PTest(CommandContext ctx, string ass, string cuck, params int[] cock)
        {
            int sum = 0;
            foreach (int num in cock)
            {
                sum += num;
            }
            await ctx.RespondAsync($"Total sum is {sum}");
        }

        [Command("sudo"), OwnerOnly]
        public async Task Sudo(CommandContext ctx, DiscordMember member, [Remainder] string command)
        {
            throw new NotImplementedException(); //how to avoid implementing stuff
        }
    }
}
