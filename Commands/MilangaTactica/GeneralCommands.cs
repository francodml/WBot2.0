using System;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using WBot2.Commands.Attributes;
using WBot2.Helpers;

namespace WBot2.Commands.MilangaTactica
{
    public class GeneralCommands : BaseCommandModule
    {
        private readonly ICommandHandler _cmdHandler;
        public GeneralCommands(IServiceProvider serviceProvider, ICommandHandler cmdHandler) : base(serviceProvider)
        {
            _cmdHandler = cmdHandler;
        }

        [Command("ping")]
        [Alias("pong", "p")]
        [NeedsPermissions(Permissions.Administrator)]
        public async Task Ping(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"{e.Author.Mention} Pong!");
        }
        [Command("say")]
        public async Task SayCommand(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"You told me to say: '{string.Join(" ", args)}'");
        }

        [OwnerOnly]
        public async Task Sudo(MessageCreateEventArgs e, List<string> args)
        {
            string username = args[0];
            var otherUser = e.Guild.Members.Values
                .Where(x => x.Nickname == username);
            _cmdHandler.ProcessCommands(_discordClient, e);
        }
    }
}
