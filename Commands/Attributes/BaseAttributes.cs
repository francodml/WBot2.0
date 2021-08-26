using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WBot2.Data;

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

        public async Task<bool> CheckAttribute(CommandContext ctx, IServiceProvider serviceProvider)
        {
            var aMember = await ctx.Guild.GetMemberAsync(ctx.Message.Author.Id);
            Permissions mPerms = ctx.Channel.PermissionsFor(aMember);
            return mPerms.HasPermission(Permission);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class OwnerOnlyAttribute : Attribute, ICheckAttribute
    {
        public string FailString => "This command can only be run by the bot owner.";
        public Task<bool> CheckAttribute(CommandContext ctx, IServiceProvider serviceProvider)
        {
            var discordOptions = serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            return Task.FromResult(ctx.Message.Author.Id == discordOptions.Owner);
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public class RemainderAttribute : Attribute
    {

    }
}
