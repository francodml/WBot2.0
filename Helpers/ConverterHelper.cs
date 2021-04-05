using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WBot2.Converters;
using DSharpPlus.EventArgs;
using WBot2.Commands;
using WBot2.Helpers.Interfaces;

namespace WBot2.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly List<IArgConverter> _converters;
        public ConverterHelper()
        {
            _converters = StaticHelpers.GetModules<IArgConverter>();
        }

        public T ConvertParameter<T>(string argument, CommandContext ctx)
        {
            var converter = (IArgConverter<T>)_converters.FirstOrDefault(x => x.GetType().IsAssignableTo(typeof(IArgConverter<T>)));
            return converter.ParseArgument(argument, ctx).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
