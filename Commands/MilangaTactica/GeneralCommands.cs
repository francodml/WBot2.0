using System;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;

namespace WBot2.Commands.MilangaTactica
{
    public class GeneralCommands : BaseCommandModule
    {
        public GeneralCommands(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        } 

        [Command("ping")]
        public async Task Ping(MessageCreateEventArgs e)
        {
            await e.Message.RespondAsync("Pong!");
        }
        [Command("pingmention")]
        public async Task MentionPing(MessageCreateEventArgs e)
        {
            await e.Message.RespondAsync($"{e.Author.Mention} Pong!");
        }
    }
}
