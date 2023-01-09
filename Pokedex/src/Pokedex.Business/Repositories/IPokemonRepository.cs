using Pokedex.Business.Core.Interfaces;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;

namespace Pokedex.Business.Repositories
{
    public interface IPokemonRepository : IRepository
    {
        public Task Add(Pokemon pokemon);
        public void Update(Pokemon pokemon);
        public void Delete(Guid pokemonId);

        public Task<bool> HasPokemon(Guid pokemonId);
        public Task<Pokemon> GetById(Guid pokemonId);
        public Task<Pokemon> GetByName(string name);
        public Task<IEnumerable<Pokemon>> Find(FindPokemonQuery query);
    }
}