using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pokedex.Business.Core.Auth;
using Pokedex.Business.Core.Notifications;
using Pokedex.Business.Repositories;
using Pokedex.Business.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Pokedex.Tests.Services
{
    public class AuthenticationServiceTests
    {
        public readonly Mock<INotifier> _notifierMock;
        public readonly Mock<IUserRepository> _userRepositoryMock;
        public readonly Mock<IOptions<AuthSettings>> _authSettingsMock;

        public AuthenticationServiceTests()
        {
            _notifierMock = new Mock<INotifier>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authSettingsMock = new Mock<IOptions<AuthSettings>>();

            _authSettingsMock.SetupGet(s => s.Value).Returns(new AuthSettings()
            {
                Secret = Guid.NewGuid().ToString()
            });
        }

        [Fact(DisplayName = "Deve lançar notificação quando o login do usuário for inválido.")]
        public async Task Login_WhenInvalidUsername()
        {
            // Arrange
            var login = new Login();
            var service = new AuthenticationService(_userRepositoryMock.Object, _authSettingsMock.Object, _notifierMock.Object);

            // Act
            var result = await service.Login(login);

            // Asserts
            _notifierMock.Verify(n => n.Notify("Usuário ou senha incorretos."), Times.Once);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve lançar notificação quando a senha do usuário for inválido.")]
        public async Task Login_WhenInvalidPassword()
        {
            // Arrange
            var login = new Login() { Password = Guid.NewGuid().ToString() };
            var user = new User() { Password = Guid.NewGuid().ToString() };

            var service = new AuthenticationService(_userRepositoryMock.Object, _authSettingsMock.Object, _notifierMock.Object);

            _userRepositoryMock.Setup(ur => ur.GetByLogin(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var result = await service.Login(login);

            // Asserts
            _notifierMock.Verify(n => n.Notify("Usuário ou senha incorretos."), Times.Once);
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve retornar um token JWT quando as credenciais do usuário forem válidas.")]
        public async Task Login_Success()
        {
            // Arrange
            var login = new Login() { Password = "1234" };

            var user = new User() 
            { 
                Username = Guid.NewGuid().ToString(), 
                Password = "81dc9bdb52d04dc20036dbd8313ed055" 
            };

            var service = new AuthenticationService(_userRepositoryMock.Object, _authSettingsMock.Object, _notifierMock.Object);

            _userRepositoryMock.Setup(ur => ur.GetByLogin(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var token = await service.Login(login);
            var claims = GetTokenClaims(token);

            // Asserts
            _notifierMock.Verify(n => n.Notify(It.IsAny<string>()), Times.Never);
            GetClaimValue(JwtRegisteredClaimNames.Sub, claims).Should().Be(user.Username);
        }

        private static IEnumerable<Claim> GetTokenClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenSecurity = jsonToken as JwtSecurityToken;

            return tokenSecurity!.Claims;
        }

        private static string GetClaimValue(string claimType, IEnumerable<Claim> claims)
        {
            return claims!.First(c => c.Type == claimType).Value;
        }
    }
}