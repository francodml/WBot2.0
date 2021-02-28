using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WBot2.Data;
using DSharpPlus;

namespace WBot2.Commands
{
    public class BaseCommandModule
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DiscordOptions _baseOptions;
        protected readonly DiscordClient _discordClient;
        public BaseCommandModule(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            _discordClient = serviceProvider.GetRequiredService<DiscordClient>();
        }
    }
}
