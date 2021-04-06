using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using WBot2.Data;
using WBot2.Helpers.Interfaces;

namespace WBot2.Helpers
{
    class ReactionRolesHelper : IReactionHelper
    {
        private readonly ReactionRoleContext _reactionrolescontext;
        public ReactionRoleContext RRDbContext { get => _reactionrolescontext; }
        public ReactionRolesHelper(ReactionRoleContext rrcontext)
        {
            _reactionrolescontext = rrcontext;
        }

        public async Task ReactionAdded(MessageReactionAddEventArgs e)
        {
            throw new NotImplementedException("doing this later kthxbye");
        }

        public async Task ReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
