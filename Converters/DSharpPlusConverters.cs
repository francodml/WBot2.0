using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WBot2.Commands;
using WBot2.Helpers;

namespace WBot2.Converters
{
    class MemberConverter : IArgConverter<DiscordMember>
    {
        async Task<DiscordMember> IArgConverter<DiscordMember>.ParseArgument(string arg, CommandContext ctx)
        {
            var members = ctx.Guild.Members;
            DiscordMember aMember = members.FirstOrDefault(x => x.Value.Mention == arg).Value;
            return aMember;
        }
    }

    public class MessageConverter : IArgConverter<DiscordMessage>
    {
        async Task<DiscordMessage> IArgConverter<DiscordMessage>.ParseArgument(string argument, CommandContext ctx)
        {
            if (argument.Length != 18)
                throw new ArgConverterException(this, "Not a message ID");
            ulong.TryParse(argument, out ulong result);
            DiscordMessage msg;
            try
            {
                msg = await ctx.Channel.GetMessageAsync(result);
                return msg;
            }
            catch(Exception e)
            {
                switch (e)
                {
                    case NotFoundException nfe:
                        throw new ArgConverterException(this, nfe.Message);
                    default:
                        break;
                }
            }
        }
    }
}
