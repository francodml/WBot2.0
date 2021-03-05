using System;
using System.Collections.Generic;
using System.Linq;
using DSharpPlus.Entities;
using System.Text;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace WBot2.Converters
{
    class MemberConverter : IArgConverter<DiscordMember>
    {
        public async Task<DiscordMember> ParseArgument(string arg, MessageCreateEventArgs e)
        {
            var members = await e.Guild.GetAllMembersAsync();
            DiscordMember aMember = members.FirstOrDefault(x => x.Mention == arg);
            return aMember;
        }
    }
}
