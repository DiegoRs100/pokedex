using Bogus;
using FluentAssertions;
using Pokedex.Business.Entities;
using Pokedex.Business.Enums;
using Xunit;

namespace Pokedex.Tests.Entities
{
    public class PokemonTests
    {
        private readonly Faker _faker;

        public PokemonTests() 
        { 
            _faker = new Faker(); 
        }

        [Fact(DisplayName = "Deve inicializar as principais propriedades quando o objeto for instanciado.")]
        public void Constructor()
        {
            //Arrange
            var name = _faker.Person.FirstName;
            var categoryId = Guid.NewGuid();
            var gender = _faker.Random.Enum<Gender>();
            var hp = _faker.Random.Number(1, 100);
            var attack = _faker.Random.Number(1, 100);
            var defense = _faker.Random.Number(1, 100);
            var speed = _faker.Random.Number(1, 100);

            //Act
            var pokemon = new Pokemon(name, categoryId, gender, hp, attack, defense, speed);

            //Asserts
            pokemon.Name.Should().Be(name);
            pokemon.CategoryId.Should().Be(categoryId);
            pokemon.Gender.Should().Be(gender);
            pokemon.Hp.Should().Be(hp);
            pokemon.Attack.Should().Be(attack);
            pokemon.Defense.Should().Be(defense);
            pokemon.Speed.Should().Be(speed);
        }

        [Fact(DisplayName = "Deve ser válido quando o pokémon for constrído corretamente.")]
        public void ValidarValidacaoDePokemonValido()
        {
            var pokemon = new Pokemon("Lucario", Guid.NewGuid(), Gender.All, 1, 1, 1, 1);
            var validate = pokemon.Validate();

            validate.IsValid.Should().BeTrue();
        }

        [Theory(DisplayName = "Deve ser inválido quando o pokémon for construído incorretamente.")]
        [InlineData(1)]
        [InlineData(51)]
        public void ValidarValidacaoDePokemonInvalido(int pokemonNameLengh)
        {
            //Arrange
            var pokemon = new Pokemon(_faker.Random.String2(pokemonNameLengh), 
                Guid.Empty, Gender.All, 0, 0, 0, 0);
                      
            //Act
            var validate = pokemon.Validate();

            //Asserts
            validate.Errors.Should().HaveCount(6);
            validate.Errors.Should().Contain(e => e.ErrorMessage == "O nome do pokémon deve ter entre 3 e 50 caracteres.");
            validate.Errors.Should().Contain(e => e.ErrorMessage == "É necessário informar uma categoria válida.");
            validate.Errors.Should().Contain(e => e.ErrorMessage == "O HP deve ser maior que zero.");
            validate.Errors.Should().Contain(e => e.ErrorMessage == "O ataque deve ser maior que zero.");
            validate.Errors.Should().Contain(e => e.ErrorMessage == "A defesa deve ser maior que zero.");
            validate.Errors.Should().Contain(e => e.ErrorMessage == "A velocidade deve ser maior que zero.");
        }
    }
}