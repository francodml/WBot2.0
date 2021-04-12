using System;
using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Task<object> ConvertParameterAsync<T>(CommandContext ctx, string argument, int argindex);
        Task<object> ConvertParameterAsync(CommandContext ctx, Type target, string argument, int argindex);

        Task<T> ConvertRemainingParamsAsync<T>(CommandContext ctx, string[] arguments);
        Task<object[]> ConvertRemainingParamsAsync(CommandContext ctx, Type target, string[] arguments);
    }
}