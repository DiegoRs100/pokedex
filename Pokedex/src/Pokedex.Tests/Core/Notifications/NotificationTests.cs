using Bogus;
using FluentAssertions;
using Pokedex.Business.Core.Notifications;
using Xunit;

namespace Pokedex.Tests.Core.Notifications
{
    public class NotificationTests
    {
        [Fact(DisplayName = "Deve inicializar as proriedades básicas quando o construtor for chamado apenas contendo a mensagem de erro.")]
        public void Constructor()
        {
            var faker = new Faker();
            var message = faker.Random.Words(5);

            var notification = new Notification(message);

            notification.Message.Should().Be(message);
        }
    }
}