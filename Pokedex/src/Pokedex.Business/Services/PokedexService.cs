using Pokedex.Business.Core.Notifications;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;
using Pokedex.Business.Repositories;

namespace Pokedex.Business.Services
{
    public class PokedexService : IPokedexService
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly INotifier _notifier;

        public PokedexService(IPokemonRepository pokemonRepository, INotifier notifier)
        {
            _notifier = notifier;
            _pokemonRepository = pokemonRepository;
        }

        public async Task<Guid?> AddPokemon(Pokemon pokemon)
        {
            var validation = pokemon.Validate();

            if (!validation.IsValid)
            {
                _notifier.Notify(validation);
                return null;
            }

            var registredPokemon = await _pokemonRepository.GetByName(pokemon.Name);

            if (registredPokemon != null)
            {
                _notifier.Notify("Já existem um pokémon cadastrado com esse nome.");
                return null;
            }

            await _pokemonRepository.Add(pokemon);
            await _pokemonRepository.SaveChanges();

            return pokemon.Id;
        }

        public async Task UpdatePokemon(Pokemon pokemon)
        {
            var validation = pokemon.Validate();

            if (!validation.IsValid)
            {
                _notifier.Notify(validation);
                return;
            }

            var hasPokemon = await _pokemonRepository.HasPokemon(pokemon.Id);

            if (!hasPokemon)
            {
                _notifier.Notify("Não foi possível encontrar o pokémon informado.");
                return;
            }

            var registredPokemon = await _pokemonRepository.GetByName(pokemon.Name);

            if (registredPokemon != null)
            {
                _notifier.Notify("Não é possível alterar o nome desse pokémon pois já existe um outro cadastrado com o mesmo nome.");
                return;
            }

            _pokemonRepository.Update(pokemon);
            await _pokemonRepository.SaveChanges();
        }

        public async Task DeletePokemon(Guid pokemonId)
        {
            var hasPokemon = await _pokemonRepository.HasPokemon(pokemonId);

            
            if (!hasPokemon)
            {
                _notifier.Notify("Não foi possível encontrar o pokémon informado.");
                return;
            }
            
            _pokemonRepository.Delete(pokemonId);
            await _pokemonRepository.SaveChanges();
        }

        public Task<Pokemon> GetPokemonById(Guid pokemonId)
        {
            return _pokemonRepository.GetById(pokemonId);
        }

        public async Task<IEnumerable<Pokemon>> FindPokemons(FindPokemonQuery query)
        {
            return await _pokemonRepository.Find(query);
        }
    }
}