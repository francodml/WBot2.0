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
        public ReactionRoleCommands(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _reactionHelper = serviceProvider.GetRequiredService<ReactionRolesHelper>();
        }

        [Command("watchmessage")]
        public async Task AddWatchedMessage()
        {
            throw new NotImplementedException();
        }
    }
}
