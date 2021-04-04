using DSharpPlus.EventArgs;
using WBot2.Data;
using System.Threading.Tasks;

namespace WBot2.Helpers.Interfaces
{
    public interface IReactionHelper
    {
        public Task ReactionAdded(MessageReactionAddEventArgs e);
        public Task ReactionRemoved(MessageReactionRemoveEventArgs e);
        public ReactionRoleContext RRDbContext {get;}
    }
}
