using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;

namespace WBot2
{
    [Description("Regular commands for everyone")]
    public class WBotCommands : BaseCommandModule
    {
        [Command("hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"Hi {ctx.User.Mention}!");
        }

        [Command("say")]
        public async Task Say(CommandContext ctx, [RemainingText] string toSay)
        {
            await ctx.RespondAsync($"You told me to say {toSay}.");
        }

        [Command("dICK]")]
        [Hidden]
        public async Task DICK(CommandContext ctx)
        {
            if (ctx.Member.Id == 155038499664297985)
                await ctx.Member.SendMessageAsync("I trust you won't leak that key.");
            else
                await ctx.RespondAsync("you are not worthy");
        }

        [Command("ping")]
        [Description("Pings the bot. It in turn, replies")]
        public async Task Ping(CommandContext ctx) => await ctx.RespondAsync("418 - I'm a teapot");
        [Command("sendembed")]
        [Description("Send an embed")]
        public async Task EmbedSend(CommandContext ctx, DiscordColor color, DiscordChannel channel)
        {
            var interact = ctx.Client.GetInteractivity();
            await ctx.RespondAsync("Mandá un mensaje con el contenido del embed");
            var msg2 = await interact.WaitForMessageAsync((DiscordMessage msg) => {
                return msg.Author == ctx.Message.Author;
            });

            var embed = new DiscordEmbedBuilder()
                .WithColor(color)
                .WithDescription(msg2.Result.Content)
                .Build();

            await new DiscordMessageBuilder()
                .WithEmbed(embed)
                .SendAsync(channel);
            await msg2.Result.RespondAsync($"Your message will be sent in {channel.Name}.");
        }

    }

    [Group("tests")]
    [Description("Testing commands from dev phase")]
    public class TestCommands : BaseCommandModule
    {
        [Command("perms")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task PermsTest(CommandContext ctx, DiscordMember member)
        {
            //ctx.Client.Logger.LogInformation($"Running permstest on {member.Username}", DateTime.Now);
            await ctx.RespondAsync($"Hola {member.Nickname}! ({member.DisplayName})");
        }
    }
}
