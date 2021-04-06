using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Converters
{
    public interface IArgConverter
    {

    }
    public interface IArgConverter<T> : IArgConverter
    {
        Task<T> ParseArgument(string argument, CommandContext ctx);
    }

}
