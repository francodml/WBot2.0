using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using WBot2.Commands;

namespace WBot2.Converters
{
    class MemberConverter : IArgConverter<DiscordMember>
    {
        async Task<DiscordMember> IArgConverter<DiscordMember>.ParseArgument(string arg, CommandContext ctx)
        {
            var members = await ctx.Guild.GetAllMembersAsync();
            DiscordMember aMember = members.FirstOrDefault(x => x.Mention == arg);
            return aMember;
        }
    }
}
