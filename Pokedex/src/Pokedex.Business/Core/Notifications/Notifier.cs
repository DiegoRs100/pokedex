using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Pokedex.Business.Core.Notifications
{
    public class Notifier : INotifier
    {
        private readonly ILogger<Notifier> _logger;
        private readonly List<Notification> _notifications = new();

        public bool HasNotifications => _notifications.Any();

        public Notifier(ILogger<Notifier> logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Notify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                 return;

            _logger.LogInformation("Notified: {message}", message);
            _notifications.Add(new Notification(message));
        }

        public void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors.Select(e => e.ErrorMessage))
            {
                _logger.LogInformation("Notified: {message}", error);

                if (string.IsNullOrWhiteSpace(error))
                    continue;

                _notifications.Add(new Notification(error));
            }
        }

        public JsonResult GetAsJsonResult()
        {
            if (!HasNotifications)
                return new JsonResult(null) { StatusCode = (int)HttpStatusCode.UnprocessableEntity };

            return new JsonResult(GetNotifications())
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
    }
}