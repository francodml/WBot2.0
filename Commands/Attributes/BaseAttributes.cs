using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WBot2.Data;
using Microsoft.Extensions.Options;

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
    public class NeedsPermissionsAttribute : Attribute, ICheckAttribute
    {
        public Permissions Permission { get; private set; }
        public string FailString => $"You lack the required permissions to run this command ({Permission.ToPermissionString()})";
        public NeedsPermissionsAttribute(Permissions perms) => this.Permission = perms;

        public async Task<bool> CheckAttribute(MessageCreateEventArgs e, IServiceProvider serviceProvider)
        {
            var aMember = await e.Guild.GetMemberAsync(e.Author.Id);
            Permissions mPerms = e.Channel.PermissionsFor(aMember);
            return mPerms.HasPermission(Permission);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OwnerOnlyAttribute : Attribute, ICheckAttribute
    {
        public string FailString => "This command can only be run by the bot owner.";
        public Task<bool> CheckAttribute(MessageCreateEventArgs e, IServiceProvider serviceProvider)
        {
            var discordOptions = serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            return Task.FromResult(e.Author.Id == discordOptions.Owner);
        }
    }
}
