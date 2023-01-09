using Pokedex.Business.Core.Interfaces;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;

namespace Pokedex.Business.Services
{
    public interface IPokedexService
    {
        public Task<Guid?> AddPokemon(Pokemon pokemon);
        public Task UpdatePokemon(Pokemon pokemon);
        public Task DeletePokemon(Guid pokemonId);
    }
}