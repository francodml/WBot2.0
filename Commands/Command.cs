﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WBot2.Extensions;
using WBot2.Commands.Attributes;

namespace WBot2.Commands
{
    public struct Command
    {
        public MethodInfo Method { init; get; }

        public BaseCommandModule RegisteringModule { init; get; }

        public string Name => Method.GetCustomAttribute<CommandAttribute>().Name;

        public string Description => Method.GetCustomAttribute<DescriptionAttribute>().Description;

        public Task Call(BaseCommandModule moduleInstance, object[] args) => (Task)Method.Invoke(moduleInstance, args);

        public T GetCustomAttribute<T>() where T : Attribute => Method.GetCustomAttribute<T>();

        public bool HasCustomAttribute<T>() where T : Attribute => Method.HasCustomAttribute<T>();

        public IEnumerable<Attribute> GetCustomAttributes() => Method.GetCustomAttributes();
    }
}