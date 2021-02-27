﻿using System;
using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WBot2.Data;
using WBot2.Services;
using WBot2.Helpers;

namespace WBot2
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var envB = new ConfigurationBuilder().AddEnvironmentVariables().AddCommandLine(args).Build();
            var env = envB.GetValue<string>("Environment");
            var builder = new HostBuilder();
            //builder.UseEnvironment(env);
            builder.ConfigureAppConfiguration(configBuilder => ConfigureAppConfiguration(configBuilder, args, env));
            builder.ConfigureLogging(ConfigureLogging);
            builder.ConfigureServices(ConfigureServices);

            return builder;
        }

        private static void ConfigureAppConfiguration(IConfigurationBuilder configBuilder, string[] args, string env)
        {
            configBuilder
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var config = hostContext.Configuration;
            services.AddTransient(_ => new DiscordClient(new DiscordConfiguration
            {
                Token = config["Discord:Token"],
                TokenType = TokenType.Bot
            }));

            services.ConfigureWithValidation<DiscordOptions>(config.GetSection("Discord"));
            services.AddHostedService<DiscordService>();
            services.AddSingleton<DiscordCommandHandler>();

        }

        private static void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
            loggingBuilder.AddConsole();

        }
    }
}
