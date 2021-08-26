using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace WBot2.Commands.Attributes
{
    public interface ICheckAttribute
    {
        public string FailString { get; }
        public Task<bool> CheckAttribute(CommandContext ctx, IServiceProvider serviceProvider);
    }
}
