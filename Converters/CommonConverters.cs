using System.Threading.Tasks;
using WBot2.Commands;
using WBot2.Helpers;

namespace WBot2.Converters
{
    class IntConverter : IArgConverter<int>
    {
        Task<int> IArgConverter<int>.ParseArgument(string argument, CommandContext ctx)
        {
            if (int.TryParse(argument, out int result))
                return Task.FromResult(result);
            throw new ArgConverterException(this, $"Cannot parse '{argument}' as an integer");
        }
    }


}
