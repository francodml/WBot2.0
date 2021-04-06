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
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;

namespace WBot2.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly Dictionary<Type, IArgConverter> _converters;
        private readonly ILogger<IConverterHelper> _logger;
        public ConverterHelper(ILogger<IConverterHelper> logger)
        {
            _converters = StaticHelpers.GetModulesWithType<IArgConverter>();
            _logger = logger;
        }

        public async Task<object> ConvertParameterAsync<T>(CommandContext ctx, string argument, int argindex)
        {
            try
            {
                IArgConverter<T> converter = (IArgConverter<T>)_converters.FirstOrDefault(x => x.Key == typeof(T)).Value;
                if (converter == null)
                {
                    throw new NotImplementedException($"No converter registered for type {typeof(T)}");
                }
                T? result = await converter.ParseArgument(argument, ctx);
                /*if (result == null)
                {
                    throw new NotSupportedException("fuck");
                }*/
                return result;
            }
            catch (ArgConverterException e)
            {
                e.ArgumentIndex = argindex;
                throw;
            }
        }

        public Task<object> ConvertParameterAsync(CommandContext ctx, Type target, string argument, int argindex)
        {
            MethodInfo generic = GetType().GetMethods().FirstOrDefault(x=> x.Name == "ConvertParameterAsync" && x.IsGenericMethod).MakeGenericMethod(target);
            return (Task<object>)generic.Invoke(this, new object[] { ctx, argument, argindex });
        }
    }

    [Serializable]
    public class ArgConverterException : Exception
    {
        public readonly IArgConverter Converter;
        public int ArgumentIndex;
        public override string Message =>
            $"Invalid argument at index {ArgumentIndex} ({base.Message})";

        public ArgConverterException(IArgConverter converter)
        {
            this.Converter = converter;
        }
        public ArgConverterException(IArgConverter converter, string message) : base(message)
        {
            this.Converter = converter;
        }
        public ArgConverterException(IArgConverter converter, string message, Exception inner) : base(message, inner)
        {
            this.Converter = converter;
        }
        protected ArgConverterException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
        public override string ToString() => Message;
    }
}
