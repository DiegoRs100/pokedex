using Microsoft.EntityFrameworkCore;
using Pokedex.Business.Core.Repositories;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;
using Pokedex.Business.Repositories;

namespace Pokedex.Infra.Repositories
{
    public class PokemonRepository : RepositoryBase, IPokemonRepository
    {
        public PokemonRepository(EFDbContext eFDbContext) : base(eFDbContext)
        { }

        public async Task Add(Pokemon pokemon)
        {
            await EFDbContext.Pokemons.AddAsync(pokemon);
        }

        public void Update(Pokemon pokemon)
        {
            EFDbContext.Pokemons.Update(pokemon);
        }

        public void Delete(Guid pokemonId)
        {
            EFDbContext.Pokemons
                .Where(p => p.Id == pokemonId)
                .ExecuteDelete();
        }

        public Task<Pokemon> GetById(Guid pokemonId)
        {
            return EFDbContext.Pokemons.FirstAsync(p => p.Id == pokemonId);
        }

        public Task<Pokemon> GetByName(string name)
        {
            return EFDbContext.Pokemons.FirstAsync(p => p.Name == name);
        }

        public Task<bool> HasPokemon(Guid pokemonId)
        {
            return EFDbContext.Pokemons.AnyAsync(p => p.Id == pokemonId);
        }

        public async Task<IEnumerable<Pokemon>> Find(FindPokemonQuery query)
        {
            var findQuery = EFDbContext.Pokemons.AsQueryable();

            if (query.HasName)
                findQuery = findQuery.Where(p => p.Name.Contains(query.Name));

            if (query.HasCategory)
                findQuery = findQuery.Where(p => p.CategoryId == query.CategoryId);

            return await findQuery.ToListAsync();
        }
    }
}