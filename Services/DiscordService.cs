using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WBot2.Data;
using WBot2.Helpers;

namespace WBot2.Services
{
    public class DiscordService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly DiscordClient _client;
        private readonly DiscordOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandHandler _commandHandler;

        public DiscordService(
            ILogger<DiscordService> logger,
            DiscordClient client,
            IOptions<DiscordOptions> options,
            IServiceProvider serviceProvider,
            ICommandHandler commandHandler
            )
        {
            _logger = logger;
            _client = client;
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _commandHandler = commandHandler;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord service is starting");

            _client.MessageCreated += async (sender, e) =>
            {
                try
                {
                    await MessageCreated(sender, e);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to run {nameof(MessageCreated)}");
                }
            }
;
            //_client.GuildMemberRemoved += GuildMemberRemovedAsync;

            await _client.ConnectAsync();
        }

        public async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
                return;
            await _commandHandler.ProcessCommands(sender, e);
            _logger.LogInformation($"User {e.Author.Username}#{e.Author.Discriminator} issued a possible command.");
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord service is stopping");

            await _client.DisconnectAsync();
        }
    }
}
