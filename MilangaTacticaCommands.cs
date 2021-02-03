using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;

namespace WBot2
{
    [Description("Commands for Milanga Táctica Capitalista uwu")]
    [RequirePermissions(Permissions.Administrator)]
    class MilangaTacticaCommands : BaseCommandModule
    {
        [Command("globalrole")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task GlobalRole(CommandContext ctx, DiscordRole role = null)
        {
            if(ctx.Guild.Id != 485547804807135242)
            {
                await ctx.RespondAsync("This command can only be ran on the bot owner's server.");
            }
            else
            {
                var interactivity = ctx.Client.GetInteractivity();
                var msg = await ctx.RespondAsync($"Confirm assigning role {role.Name} to everyone. You have 10 seconds.");

                var emoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");

                await msg.CreateReactionAsync(emoji);
                var result = await interactivity.WaitForReactionAsync(msg, ctx.User, TimeSpan.FromSeconds(10));

                if (!result.TimedOut)
                {
                    await ctx.RespondAsync("Proceeding...");
                    ctx.Client.Logger.LogInformation("Filtering members");
                    var members = ctx.Guild.Members.Values.Where(x => !x.IsBot);
                    ctx.Client.Logger.LogInformation("Assigning roles to: ");
                    foreach (DiscordMember member in members)
                        ctx.Client.Logger.LogInformation(member.DisplayName);
                    foreach (DiscordMember member in members)
                    {
                        try
                        {
                            await member.GrantRoleAsync(role);
                        }
                        catch (Exception ex)
                        {
                            ctx.Client.Logger.LogInformation("WBot2 - GlobalRole", $"Failed assigning role to {member.DisplayName}\n {ex.Message}");
                            return;
                        }
                        await Task.Delay(2000);
                        ctx.Client.Logger.LogInformation("Assigning role to next user.");
                    };

                    await ctx.RespondAsync($"Assigned role to all {members.Count()} member/s.");
                }
                else
                {
                    await ctx.RespondAsync("Role assignment cancelled.");
                    await ctx.Channel.DeleteMessageAsync(msg);
                }
            }
        }
    }
}
