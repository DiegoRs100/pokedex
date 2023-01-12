using Pokedex.Business.Entities;

namespace Pokedex.Business.Services
{
    public interface IPokedexService
    {
        public Task<Guid?> AddPokemon(Pokemon pokemon);
        public Task UpdatePokemon(Pokemon pokemon);
        public Task DeletePokemon(Guid pokemonId);
    }
}