using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WBot2.Data;
using WBot2.Helpers.Interfaces;

namespace WBot2.Services
{
    public class DiscordService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly DiscordClient _client;
        private readonly DiscordOptions _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommandHandler _commandHandler;
        private readonly IReactionHelper _reactionHelper;

        public DiscordService(
            ILogger<DiscordService> logger,
            DiscordClient client,
            IOptions<DiscordOptions> options,
            IServiceProvider serviceProvider,
            ICommandHandler commandHandler,
            IReactionHelper reactionHelper
            )
        {
            _logger = logger;
            _client = client;
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _commandHandler = commandHandler;
            _reactionHelper = reactionHelper;
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

            _client.MessageReactionAdded += async (sender, e) =>
            {
                try
                {
                    await MessageReaction(sender, e);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to run {nameof(MessageReaction)}");
                }
            };

            await _client.ConnectAsync();
        }

        public Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            if (e.Author.IsBot)
                return Task.CompletedTask;
            _commandHandler.ProcessCommand(sender, e);
            return Task.CompletedTask;
        }

        public async Task MessageReaction(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            await _reactionHelper.ReactionAdded(e);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord service is stopping");

            await _client.DisconnectAsync();
        }
    }
}
