using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WBot2.Commands
{
    public class CommandContext
    {
        #region Properties
        public DiscordClient Client { init; get; }
        public DiscordMessage Message { init; get; }
        public DiscordMember Member =>
            this.lazyMember.Value;
        public DiscordChannel Channel =>
            this.Message.Channel;
        public DiscordGuild Guild =>
            this.Channel.Guild;
        public DiscordUser User =>
            this.Message.Author;
        public List<string> RawArguments { init; get; }

        public Command Command { init; get; }
        #endregion

        #region Fields
        private readonly Lazy<DiscordMember> lazyMember;
        #endregion

        #region Constructor
        public CommandContext()
        {
            this.lazyMember = new Lazy<DiscordMember>(() => {
                return this.Guild != null && this.Guild.Members.TryGetValue(this.User.Id, out var member) ? member : this.Guild?.GetMemberAsync(this.User.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            });
        }
        #endregion

        #region Methods
        public Task<DiscordMessage> RespondAsync(string msg) =>
            this.Message.RespondAsync(msg);
        public Task<DiscordMessage> RespondAsync(DiscordEmbed embed) =>
            this.Message.RespondAsync(embed);
        public Task<DiscordMessage> RespondAsync(string content, DiscordEmbed embed) =>
            this.Message.RespondAsync(content, embed);
        public Task<DiscordMessage> RespondAsync(DiscordMessageBuilder builder) =>
            this.Message.RespondAsync(builder);

        public Task TriggerTypingAsync() =>
            this.Channel.TriggerTypingAsync();
        #endregion

    }
}
