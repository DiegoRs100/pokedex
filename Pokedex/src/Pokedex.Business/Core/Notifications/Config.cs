using Microsoft.Extensions.DependencyInjection;
using Pokedex.Business.Core.Notifications.Filters;

namespace Pokedex.Business.Core.Notifications
{
    public static class Config
    {
        public static IServiceCollection AddSmartNotification(this IServiceCollection services)
        {
            services.AddScoped<INotifier, Notifier>();
            services.AddMvcCore(options => options.Filters.Add<NotificationsFilter>());

            return services;
        }
    }
}