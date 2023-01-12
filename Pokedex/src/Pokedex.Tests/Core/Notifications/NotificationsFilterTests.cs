using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Business.Core.Notifications.Filters;
using Pokedex.Business.Core.Notifications;
using Xunit;
using Pokedex.Tests.Common;
using FluentAssertions;

namespace Pokedex.Tests.Core.Notifications
{
    public class NotificationsFilterTests
    {
        private readonly Mock<INotifier> _notifierMock;
        private readonly Mock<ILogger<Notifier>> _loggerMock;

        public NotificationsFilterTests()
        {
            _notifierMock = new Mock<INotifier>();
            _loggerMock = new Mock<ILogger<Notifier>>();
        }

        [Fact(DisplayName = "Deve finalizar o filter sem qualquer ação quando o notifier não possuir notificações")]
        public void OnActionExecuting_WhenNoHasNotifications()
        {
            var actionExecutingContext = FilterContextFactory.CreateContext();
            var filter = new NotificationsFilter(_notifierMock.Object);

            filter.OnActionExecuting(actionExecutingContext);

            actionExecutingContext.Result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve ajustar o resultado da requisição para JsonResult quando houverem notificações no notifier.")]
        public void OnActionExecuting_WhenHasNotifications()
        {
            // Arrange
            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(Guid.NewGuid().ToString());

            var actionExecutingContext = FilterContextFactory.CreateContext();
            var filter = new NotificationsFilter(notifier);

            // Act
            filter.OnActionExecuting(actionExecutingContext);

            // Asserts
            actionExecutingContext.Result.Should().BeEquivalentTo(notifier.GetAsJsonResult());
        }
    }
}