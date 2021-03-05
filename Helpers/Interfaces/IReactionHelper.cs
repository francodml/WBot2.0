using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace WBot2.Helpers.Interfaces
{
    interface IReactionHelper
    {
        public Task HandleReactionAdded(MessageReactionAddEventArgs e);
        public Task ReactionRemoved(MessageReactionRemoveEventArgs e);
    }
}
