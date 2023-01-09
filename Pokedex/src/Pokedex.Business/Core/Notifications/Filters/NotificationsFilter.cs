using Microsoft.AspNetCore.Mvc.Filters;

namespace Pokedex.Business.Core.Notifications.Filters
{
    public class NotificationsFilter : IActionFilter
    {
        private readonly INotifier _notifier;

        public NotificationsFilter(INotifier notifier)
        {
            _notifier = notifier;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Not implemented.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!_notifier.HasNotifications)
                return;

            context.Result = _notifier.GetAsJsonResult();
        }
    }
}