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

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    sealed class AliasAttribute : Attribute
    {
        public string[] Aliases { get; private set; }
        // This is a positional argument
        public AliasAttribute(params string[] aliases)
        {
            this.Aliases = aliases;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public DescriptionAttribute(string description) => Description = description;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class NeedsPermissionsAttribute : Attribute
    {
        public DSharpPlus.Permissions Permissions { get; private set; }
        public NeedsPermissionsAttribute(DSharpPlus.Permissions perms) => this.Permissions = perms;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OwnerOnlyAttribute : Attribute
    {

    }
}
