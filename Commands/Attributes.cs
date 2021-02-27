using System;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;

namespace WBot2.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; private set; }
        public CommandAttribute(string name) => this.Name = name;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionsAttribute : Attribute
    {
        public DSharpPlus.Permissions Permissions { get; private set; }
        public PermissionsAttribute(DSharpPlus.Permissions perms) => this.Permissions = perms;
    }
}
