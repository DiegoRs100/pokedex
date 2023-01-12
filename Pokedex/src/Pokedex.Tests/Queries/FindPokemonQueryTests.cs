using FluentAssertions;
using Pokedex.Business.Queries;
using Xunit;

namespace Pokedex.Tests.Queries
{
    public class FindPokemonQueryTests
    {
        [Fact(DisplayName = "Deve retornar verdeiro quando o nome do pokémon for informado.")]
        public void HasName_BeTrue()
        {
            var query = new FindPokemonQuery()
            {
                Name = Guid.NewGuid().ToString()
            };

            query.HasName.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve retornar falso quando o nome do pokémon não for informado.")]
        public void HasName_BeFalse()
        {
            var query = new FindPokemonQuery();
            query.HasName.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve retornar verdeiro quando a categoria do pokémon for informada.")]
        public void HasCategory_BeTrue()
        {
            var query = new FindPokemonQuery()
            {
                CategoryId = Guid.NewGuid()
            };

            query.HasCategory.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve retornar falso quando a categoria do pokémon não for informada.")]
        public void HasCategory_BeFalse()
        {
            var query = new FindPokemonQuery();
            query.HasCategory.Should().BeFalse();
        }
    }
}