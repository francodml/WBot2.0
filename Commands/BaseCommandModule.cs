using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using WBot2.Helpers.Interfaces;
using WBot2.Data;
using DSharpPlus;

namespace WBot2.Commands
{
    public class BaseCommandModule
    {
        public virtual string ModuleName { get => this.GetType().Name; }
        protected readonly IServiceProvider _serviceProvider;
        protected readonly DiscordOptions _baseOptions;
        protected readonly DiscordClient _discordClient;
        protected ICommandHandler RegisteringHandler { get; }
        public BaseCommandModule(IServiceProvider serviceProvider, ICommandHandler commandHandler)
        {
            _serviceProvider = serviceProvider;
            this.RegisteringHandler = commandHandler;
            _baseOptions = _serviceProvider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            _discordClient = _serviceProvider.GetRequiredService<DiscordClient>();
        }
    }
}
