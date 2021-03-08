using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBot2.Helpers.Interfaces;
using DSharpPlus.EventArgs;
using DSharpPlus;

namespace WBot2.Helpers
{
    class ReactionRolesHelper : IReactionHelper
    {
        public async Task ReactionAdded(MessageReactionAddEventArgs e)
        {

        }

        public async Task ReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
