using FluentAssertions;
using Pokedex.Tests.Common;
using Xunit;

namespace Pokedex.Tests.Core
{
    public class EntityTests
    {
        [Fact(DisplayName = "Deve inicializar o Id quando o objeto for instanciado.")]
        public void Constructor()
        {
            var pokemon = new EntityWithoutValidation();
            pokemon.Id.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Deve lançar uma exception quando o objeto herdeiro não der override no método Validate.")]
        public void Validate()
        {
            var entity = new EntityWithoutValidation();

            entity.Invoking(e => e.Validate()).Should()
                .Throw<NotImplementedException>()
                .WithMessage("Override the validate method with valid conditions.");
        }
    }
}