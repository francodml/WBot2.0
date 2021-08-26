using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;
using WBot2.Extensions;

namespace WBot2.Commands
{
    public class Command
    {
        public MethodInfo Method { init; get; }
        public ParameterInfo[] Parameters =>
            Method.GetParameters();

        public CommandModule Module { init; get; }

        public string Name => Method.GetCustomAttribute<CommandAttribute>().Name;

#nullable enable
        public string[]? Aliases => Method.GetCustomAttribute<AliasAttribute>()?.Aliases;
        public string? Description => Method.GetCustomAttribute<DescriptionAttribute>()?.Description;
#nullable disable

        public Task Call(CommandModule moduleInstance, object[] args) => (Task)Method.Invoke(moduleInstance, args);

        public T GetCustomAttribute<T>() where T : Attribute => Method.GetCustomAttribute<T>();

        public bool HasCustomAttribute<T>() where T : Attribute => Method.HasCustomAttribute<T>();

        public IEnumerable<Attribute> GetCustomAttributes() => Method.GetCustomAttributes();
    }
}