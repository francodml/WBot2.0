using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Threading.Tasks;

namespace WBot2
{

    class Program
    {
        static DiscordClient discord;
        static CommandsNextExtension commands;
        static InteractivityExtension interactivity;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            //load JSON settings

            SettingsManager.LoadSettings();
            var cfg = SettingsManager.Cfg;

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = cfg.Token,
                TokenType = TokenType.Bot,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
            });
            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "w:" },
                EnableDms = true,
                EnableMentionPrefix = true
            });
            interactivity = discord.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = PaginationBehaviour.Ignore,


                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(2)
            });



            commands.RegisterCommands<WBotCommands>();
            commands.RegisterCommands<TestCommands>();
            commands.RegisterCommands<MilangaTacticaCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
