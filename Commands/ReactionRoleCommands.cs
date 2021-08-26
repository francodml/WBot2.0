using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;
using WBot2.Data;
using WBot2.Helpers.Interfaces;

namespace WBot2.Commands
{
    class ReactionRoleCommands : CommandModule
    {
        private readonly IReactionHelper _reactionHelper;
        public ReactionRoleCommands(IServiceProvider serviceProvider, ICommandHandler commandHandler) : base(serviceProvider, commandHandler)
        {
            _reactionHelper = serviceProvider.GetRequiredService<IReactionHelper>();
        }

        public override string ModuleName => "Reaction Roles";

        [Command("watchmessage"), Description("Adds the specified message to the list of messages to watch for reaction changes")]
        public async Task AddWatchedMessage(CommandContext ctx, List<string> args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                await ctx.Message.RespondAsync("You must provide a message ID");
            }
            if (_reactionHelper.RRDbContext.Guilds.Where(x => x.ID == ctx.Guild.Id).Count() == 0)
            {
                _reactionHelper.RRDbContext.Add(new Guild { ID = ctx.Guild.Id });
                _reactionHelper.RRDbContext.SaveChanges();
            }
            var guild = _reactionHelper.RRDbContext.Guilds
                .FirstOrDefault(x => x.ID == ctx.Guild.Id);
            guild.ReactMessages.Add(new RRMessage { ID = ulong.Parse(args[0]), GuildID = ctx.Guild.Id });
            await ctx.Message.RespondAsync($"Added message with ID {args[0]} to the watched messages table.\nConfigure it further with the modify command.");
        }
    }
}
