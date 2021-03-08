using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace WBot2.Helpers.Interfaces
{
    public interface IReactionHelper
    {
        public Task ReactionAdded(MessageReactionAddEventArgs e);
        public Task ReactionRemoved(MessageReactionRemoveEventArgs e);
    }
}
