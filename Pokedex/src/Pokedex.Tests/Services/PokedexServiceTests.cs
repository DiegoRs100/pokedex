using Bogus;
using FluentAssertions;
using Moq;
using Pokedex.Business.Core.Notifications;
using Pokedex.Business.Entities;
using Pokedex.Business.Enums;
using Pokedex.Business.Repositories;
using Pokedex.Business.Services;
using Xunit;
using FluentValidationResult = FluentValidation.Results.ValidationResult;

namespace Pokedex.Tests.Services
{
    public class PokedexServiceTests
    {
        private readonly Mock<IPokemonRepository> _pokemonRepository;
        private readonly Mock<INotifier> _notifier;
        private readonly Faker _faker;

        public PokedexServiceTests()
        {
            _pokemonRepository = new Mock<IPokemonRepository>();
            _notifier = new Mock<INotifier>();
            _faker = new Faker();
        }

        [Fact(DisplayName = "Deve lançar notificações e impedir o cadastro quando o pokemon informado não for válido.")]
        public async Task AddPokemon_WhenPokemonInvalid()
        {
            // Arrange
            var pokemon = new Pokemon(string.Empty, Guid.Empty, Gender.All, 1, 1, 1, 1);
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            var result = await service.AddPokemon(pokemon);

            // Asserts
            _notifier.Verify(n => n.Notify(It.IsAny<FluentValidationResult>()), Times.Once);
            _pokemonRepository.Verify(pr => pr.GetByName(It.IsAny<string>()), Times.Never);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve lançar uma notificação e impedir o cadastro quando já existir um pokémon com o mesmo nome informado.")]
        public async Task AddPokemon_WhenPokemonDuplicated()
        {
            // Arrange
            var pokemon = new Pokemon(Guid.NewGuid().ToString(), Guid.NewGuid(), _faker.Random.Enum<Gender>(), 1, 1, 1, 1);
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            _pokemonRepository.Setup(pr => pr.GetByName(It.IsAny<string>()))
                .ReturnsAsync(new Pokemon(string.Empty, Guid.Empty, Gender.All, 1, 1, 1, 1));

            // Act
            var result = await service.AddPokemon(pokemon);

            // Asserts
            _notifier.Verify(n => n.Notify("Já existem um pokémon cadastrado com esse nome."), Times.Once);
            _pokemonRepository.Verify(pr => pr.Add(It.IsAny<Pokemon>()), Times.Never);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve cadastrar o pokémon com sucesso quando ele passar em todas as validações.")]
        public async Task AddPokemon_WhenSuccess()
        {
            // Arrange
            var pokemon = new Pokemon(Guid.NewGuid().ToString(), Guid.NewGuid(), _faker.Random.Enum<Gender>(), 1, 1, 1, 1);
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            var result = await service.AddPokemon(pokemon);

            // Asserts
            _pokemonRepository.Verify(pr => pr.Add(pokemon), Times.Once);
            _pokemonRepository.Verify(pr => pr.SaveChanges(), Times.Once);

            result.Should().Be(pokemon.Id);
        }

        // Deve lançar notificações e impedir o update quando o pokemon informado não for válido.
        // Deve lançar uma notificação e impedir o update quando o pokémon não for encontrado no banco.
        // Deve lançar uma notificação e impedir o update quando já existir um cadastro com o mesmo nome do pokémon informado.
        // Deve atualizar o pokémon no banco quando existir um cadastro prévio do mesmo.

        // DeletePokemon - Deve lançar uma notificação e impedir a remoção quando o pokémon não for encontrado no banco.
        // DeletePokemon - Deve remover o pokémon do banco quando existir um cadastro prévio do mesmo.
    }
}