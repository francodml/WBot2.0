using System.Threading.Tasks;

namespace WBot2.Helpers.Interfaces
{
    public interface IHelpFormatter<T>
    {
        public abstract Task<T> FormatHelp(ICommandHandler commandHandler);
    }
}