using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using WBot2.Data;

namespace WBot2.Helpers.Interfaces
{
    public interface IReactionHelper
    {
        public Task ReactionAdded(MessageReactionAddEventArgs e);
        public Task ReactionRemoved(MessageReactionRemoveEventArgs e);
        public ReactionRoleContext RRDbContext {get;}
    }
}
