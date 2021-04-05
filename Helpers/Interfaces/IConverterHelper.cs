using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        T ConvertParameter<T>(string argument, CommandContext ctx);
    }
}