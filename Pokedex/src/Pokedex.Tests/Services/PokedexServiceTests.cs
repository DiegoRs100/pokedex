using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Pokedex.Business.Core.Notifications;
using Pokedex.Business.Entities;
using Pokedex.Business.Enums;
using Pokedex.Business.Repositories;
using Pokedex.Business.Services;
using Pokedex.Tests.Common;
using Xunit;

namespace Pokedex.Tests.Services
{
    public class PokedexServiceTests
    {
        private readonly Mock<IPokemonRepository> _pokemonRepository;
        private readonly Mock<INotifier> _notifier;

        public PokedexServiceTests()
        {
            _pokemonRepository = new Mock<IPokemonRepository>();
            _notifier = new Mock<INotifier>();
        }

        [Fact(DisplayName = "Deve lançar notificações e impedir o cadastro quando o pokemon informado não for válido.")]
        public async Task AddPokemon_WhenPokemonInvalid()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateInvalid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            var result = await service.AddPokemon(pokemon);

            // Asserts
            _notifier.Verify(n => n.Notify(It.IsAny<ValidationResult>()), Times.Once);
            _pokemonRepository.Verify(pr => pr.GetByName(It.IsAny<string>()), Times.Never);

            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve lançar uma notificação e impedir o cadastro quando já existir um pokémon com o mesmo nome informado.")]
        public async Task AddPokemon_WhenPokemonDuplicated()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
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
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            var result = await service.AddPokemon(pokemon);

            // Asserts
            _pokemonRepository.Verify(pr => pr.Add(pokemon), Times.Once);
            _pokemonRepository.Verify(pr => pr.SaveChanges(), Times.Once);

            result.Should().Be(pokemon.Id);
        }

        [Fact(DisplayName = "Deve lançar notificações e impedir o update quando o pokémon informado não for válido.")]
        public async Task UpdatePokemon_WhenPokemonInvalid()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateInvalid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            await service.UpdatePokemon(pokemon);

            // Asserts
            _notifier.Verify(n => n.Notify(It.IsAny<ValidationResult>()), Times.Once);
            _pokemonRepository.Verify(pr => pr.HasPokemon(pokemon.Id), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar uma notificação e impedir o update quando o pokémon não for encontrado no banco.")]
        public async Task UpdatePokemon_WhenPokemonNotFound()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            await service.UpdatePokemon(pokemon);

            // Asserts
            _pokemonRepository.Verify(pr => pr.HasPokemon(pokemon.Id), Times.Once);
            _notifier.Verify(n => n.Notify("Não foi possível encontrar o pokémon informado."), Times.Once);
            _pokemonRepository.Verify(pr => pr.GetByName(pokemon.Name), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar uma notificação e impedir o update " +
            "quando já existir um cadastro com o mesmo nome do pokémon informado.")]
        public async Task UpdatePokemon_WhenPokemonNameDuplicated()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            _pokemonRepository.Setup(pr => pr.HasPokemon(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _pokemonRepository.Setup(pr => pr.GetByName(It.IsAny<string>()))
                .ReturnsAsync(pokemon);

            // Act
            await service.UpdatePokemon(pokemon);

            // Asserts
            _notifier.Verify(n => n.Notify("Não é possível alterar o nome desse pokémon pois já existe um outro cadastrado com o mesmo nome."), Times.Once);
            _pokemonRepository.Verify(pr => pr.Update(It.IsAny<Pokemon>()), Times.Never);
        }

        [Fact(DisplayName = "Deve atualizar o pokémon no banco quando existir um cadastro prévio do mesmo.")]
        public async Task UpdatePokemon_WhenSuccess()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            _pokemonRepository.Setup(pr => pr.HasPokemon(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            await service.UpdatePokemon(pokemon);

            // Asserts
            _pokemonRepository.Verify(pr => pr.HasPokemon(pokemon.Id), Times.Once);
            _pokemonRepository.Verify(pr => pr.GetByName(pokemon.Name), Times.Once);
            _pokemonRepository.Verify(pr => pr.Update(pokemon), Times.Once);
            _pokemonRepository.Verify(pr => pr.SaveChanges(), Times.Once);
        }

        [Fact(DisplayName = "Deve lançar uma notificação e impedir a remoção quando o pokémon não for encontrado no banco.")]
        public async Task DeletePokemon_WhenPokemonNotFindable()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            // Act
            await service.DeletePokemon(pokemon.Id);

            // Asserts
            _notifier.Verify(n => n.Notify("Não foi possível encontrar o pokémon informado."), Times.Once);
            _pokemonRepository.Verify(pr => pr.Delete(pokemon.Id), Times.Never);
        }

        [Fact(DisplayName = "Deve remover o pokémon do banco quando existir um cadastro prévio do mesmo.")]
        public async Task DeletePokemon_WhenExistingPokemon()
        {
            // Arrange
            var pokemon = PokemonFactory.CreateValid();
            var service = new PokedexService(_pokemonRepository.Object, _notifier.Object);

            _pokemonRepository.Setup(pr => pr.HasPokemon(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            await service.DeletePokemon(pokemon.Id);

            // Asserts
            _pokemonRepository.Verify(pr => pr.Delete(pokemon.Id), Times.Once);
            _pokemonRepository.Verify(pr => pr.SaveChanges(), Times.Once);
        }
    }
}