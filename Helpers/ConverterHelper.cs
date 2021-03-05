using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WBot2.Converters;
using DSharpPlus.EventArgs;

namespace WBot2.Helpers
{
    public class ConverterHelper
    {
        private readonly List<IArgConverter> _converters;
        public ConverterHelper()
        {
            _converters = StaticHelpers.GetModules<IArgConverter>(null);
        }

        public async Task<T> ConvertParameterAsync<T> (string argument, MessageCreateEventArgs e)
        {
            var converter = (IArgConverter<T>)_converters.FirstOrDefault(x => x.GetType().IsAssignableTo(typeof(IArgConverter<T>)));
            return await converter.ParseArgument(argument, e);
        }
    }
}
