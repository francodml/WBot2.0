using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace WBot2.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureWithValidation<TOptions>(this IServiceCollection services, IConfiguration config) where TOptions : class
        {
            var options = config.Get<TOptions>();
            Validator.ValidateObject(options, new ValidationContext(options), true);

            return services.Configure<TOptions>(config);
        }
    }
}
