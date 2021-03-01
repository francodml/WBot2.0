using System;
using DSharpPlus;
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
        public Permissions Permission { get; private set; }
        public NeedsPermissionsAttribute(Permissions perms) => this.Permission = perms;
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OwnerOnlyAttribute : Attribute
    {

    }
}
