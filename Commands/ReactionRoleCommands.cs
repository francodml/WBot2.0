using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WBot2.Commands.Attributes;
using WBot2.Helpers.Interfaces;
using WBot2.Helpers;

namespace WBot2.Commands
{
    class ReactionRoleCommands : BaseCommandModule
    {
        private readonly ReactionRolesHelper _reactionHelper;
        public ReactionRoleCommands(IServiceProvider serviceProvider, ICommandHandler commandHandler) : base(serviceProvider, commandHandler)
        {
            _reactionHelper = serviceProvider.GetRequiredService<ReactionRolesHelper>();
        }

        public override string ModuleName => "Reaction Roles";

        [Command("watchmessage"), Description("Adds the specified message to the list of messages to watch for reaction changes")]
        public async Task AddWatchedMessage()
        {
            throw new NotImplementedException();
        }
    }
}
