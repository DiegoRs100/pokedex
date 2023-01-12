using Bogus;
using FluentAssertions;
using Pokedex.Business.Core.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Pokedex.Tests.Core.Auth
{
    public class JwtHelperTests
    {
        [Fact(DisplayName = "Deve gerar um Token JWT quando passado dados de um usuário válido.")]
        public void GenerateTokenJwt()
        {
            // Arrange
            var faker = new Faker();
            var username = faker.Person.Email;
            
            var settings = new AuthSettings()
            {
                Secret = Guid.NewGuid().ToString(),
                Issuer = faker.Internet.Url(),
                ValidIn = faker.Internet.Url()
            };

            // Act
            var token = JwtHelper.GenerateTokenJwt(username, settings);
            var claims = GetTokenClaims(token);

            var tokenId = new Guid(GetClaimValue(JwtRegisteredClaimNames.Jti, claims));

            var tokenInitialValidate = DateTimeOffset.FromUnixTimeSeconds(
                int.Parse(GetClaimValue(JwtRegisteredClaimNames.Nbf, claims)));

            var tokenCreationDate = DateTimeOffset.FromUnixTimeSeconds(
                int.Parse(GetClaimValue(JwtRegisteredClaimNames.Iat, claims)));

            // Asserts
            GetClaimValue(JwtRegisteredClaimNames.Sub, claims).Should().Be(username);
            GetClaimValue(JwtRegisteredClaimNames.UniqueName, claims).Should().Be(username);
            tokenId.Should().NotBeEmpty();
            tokenInitialValidate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            tokenCreationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
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