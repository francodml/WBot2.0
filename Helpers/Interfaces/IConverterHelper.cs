using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Task<T> ConvertParameterAsync<T>(string argument, CommandContext ctx);
    }
}