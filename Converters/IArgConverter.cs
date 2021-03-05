using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace WBot2.Converters
{
    public interface IArgConverter
    {

    }
    public interface IArgConverter<T> : IArgConverter
    {
        Task<T> ParseArgument(string argument, MessageCreateEventArgs e);
    }

}
