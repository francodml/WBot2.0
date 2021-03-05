using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WBot2.Helpers.Interfaces
{
    public interface ICommandHandler
    {
        Task ProcessCommands(DiscordClient sender, MessageCreateEventArgs e);
    }
}
