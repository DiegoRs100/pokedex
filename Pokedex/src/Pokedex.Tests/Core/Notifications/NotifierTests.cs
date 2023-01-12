using Bogus;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Business.Core.Notifications;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Pokedex.Tests.Core.Notifications
{
    public class NotifierTests
    {
        private readonly Mock<ILogger<Notifier>> _loggerMock;

        public NotifierTests()
        {
            _loggerMock = new Mock<ILogger<Notifier>>();
        }

        [Fact(DisplayName = "Deve retornar verdadeiro quando existirem notificações.")]
        public void HasNotifications_BeTrue()
        {
            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(Guid.NewGuid().ToString());

            notifier.HasNotifications.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve retornar falso quando não existirem notificações.")]
        public void HasNotifications_BeFalse()
        {
            var notifier = new Notifier(_loggerMock.Object);
            notifier.HasNotifications.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve retornar as notificações quando existirem notificações.")]
        public void GetNotifications()
        {
            var message = Guid.NewGuid().ToString();

            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(message);

            notifier.GetNotifications().Should().HaveCount(1)
                .And.Contain(n => n.Message == message);
        }

        [Theory(DisplayName = "Não deve realizar ações quando a mensagem for inválida - OnlyMessage.")]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Notify_Invalid_OnlyMessage(string message)
        {
            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(message);

            notifier.HasNotifications.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve adicionar notificação quando a mensagem for válida - OnlyMessage.")]
        public void Notify_Valid_OnlyMessage()
        {
            var faker = new Faker();
            var message = faker.Random.Words(10);

            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(message);

            notifier.GetNotifications().Should().HaveCount(1)
                .And.Contain(n => n.Message == message);
        }

        [Fact(DisplayName = "Deve adicionar notificação quando o (ValidationResult) contiver erros.")]
        public void Notify_ValidationResult()
        {
            var faker = new Faker();
            var validationResult = new ValidationResult();

            validationResult.Errors.Add(new ValidationFailure()
            {
                ErrorCode = Guid.NewGuid().ToString(),
                PropertyName = faker.Random.Word(),
                ErrorMessage = faker.Random.Words(10)
            });

            validationResult.Errors.Add(new ValidationFailure()
            {
                ErrorCode = Guid.NewGuid().ToString(),
                PropertyName = faker.Random.Word(),
                ErrorMessage = faker.Random.Words(10)
            });

            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(validationResult);

            notifier.GetNotifications().Should().HaveCount(2);

            notifier.GetNotifications().Should().Contain(n =>
                n.Message == validationResult.Errors[0].ErrorMessage);

            notifier.GetNotifications().Should().Contain(n =>
                n.Message == validationResult.Errors[1].ErrorMessage);
        }

        [Fact(DisplayName = "Deve retornar um JsonResult genérico quando não houverem notificações.")]
        public void GetAsJsonResult_WhenNoHasNotifications()
        {
            var notifier = new Notifier(_loggerMock.Object);
            var result = notifier.GetAsJsonResult();

            result.Value.Should().BeNull();
            result.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
        }

        [Fact(DisplayName = "Deve retornar as notificações em um JsonResult quando houverem notificações.")]
        public void GetAsJsonResult_WhenHasNotifications()
        {
            var faker = new Faker();
            var message = faker.Random.Words(10);

            var notifier = new Notifier(_loggerMock.Object);
            notifier.Notify(message);

            var result = notifier.GetAsJsonResult();

            result.Value.Should().BeEquivalentTo(notifier.GetNotifications());
            result.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
        }
    }
}