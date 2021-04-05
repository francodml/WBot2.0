using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Converters
{
    class IntConverter : IArgConverter<int>
    {
        Task<int> IArgConverter<int>.ParseArgument(string argument, CommandContext ctx)
        {
            if (int.TryParse(argument, out int result))
                return Task.FromResult(result);
            throw new Exception("i somehow can't parse this xoxo");
        }
    }
}
