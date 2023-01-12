using Bogus;
using FluentAssertions;
using Pokedex.Business.Core.Auth;
using Xunit;

namespace Pokedex.Tests.Core.Auth
{
    public class CryptographyHelperTests
    {
        private readonly Faker _faker;

        public CryptographyHelperTests()
        {
            _faker = new Faker();
        }

        [Fact(DisplayName = "Deve retornar o mesmo resultado quando uma string for passada como parâmetro mais de uma vez.")]
        public void EncryptToMD5_MultipleCall()
        {
            var text = _faker.Random.Word();

            var result1 = CryptographyHelper.EncryptToMD5(text);
            var result2 = CryptographyHelper.EncryptToMD5(text);

            result1.Should().Be(result2);
        }

        [Fact(DisplayName = "Deve retornar uma string no formato MD5 quando uma texto for passado como parâmetro.")]
        public void EncryptToMD5_CheckFormat()
        {
            var text = _faker.Random.Word();
            var result = CryptographyHelper.EncryptToMD5(text);

            result.Should().MatchRegex("^[0-9a-fA-F]{32}$");
        }
    }
}