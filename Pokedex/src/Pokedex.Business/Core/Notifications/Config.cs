using Microsoft.Extensions.DependencyInjection;
using Pokedex.Business.Core.Notifications.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Pokedex.Business.Core.Notifications
{
    public static class Config
    {
        [ExcludeFromCodeCoverage]
        public static IServiceCollection AddSmartNotification(this IServiceCollection services)
        {
            services.AddScoped<INotifier, Notifier>();
            services.AddMvcCore(options => options.Filters.Add<NotificationsFilter>());

            return services;
        }
    }
}