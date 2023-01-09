using MediatR;

namespace Pokedex.Business.Core.Notifications
{
    public class Notification : INotification
    {
        public string Message { get; }

        public Notification(string message)
        {
            Message = message;
        }
    }
}