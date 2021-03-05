using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WBot2.Commands.Attributes;
using WBot2.Helpers.Interfaces;

namespace WBot2.Commands
{
    class ReactionRoleCommands : BaseCommandModule
    {
        private readonly IReactionHelper _reactionHelper;
        public ReactionRoleCommands(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _reactionHelper = serviceProvider.GetRequiredService<IReactionHelper>();
        }

        [Command("watchmessage")]
        public async Task AddWatchedMessage()
        {
            throw new NotImplementedException();
        }
    }
}
