using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WBot2.Commands.Attributes;

namespace WBot2.Commands.MilangaTactica
{
    public class GeneralCommands : BaseCommandModule
    {
        protected readonly ILogger _logger;

        public GeneralCommands(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<GeneralCommands>>();
        }

        [Command("ping")]
        [Alias("pong", "p")]
        public async Task Ping(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"{e.Author.Mention} Pong!");
        }
        [Command("say")]
        public async Task Say(MessageCreateEventArgs e, List<string> args)
        {
            await e.Message.RespondAsync($"You told me to say: '{string.Join(" ", args)}'");
        }

        [Command("uwu")]
        [NeedsPermissions(DSharpPlus.Permissions.Administrator | DSharpPlus.Permissions.BanMembers)]
        public async Task Uwu(MessageCreateEventArgs e, List<string> args)
        {
            await Task.Delay(10000);
            await e.Message.RespondAsync("UwU");
        }
    }
}
