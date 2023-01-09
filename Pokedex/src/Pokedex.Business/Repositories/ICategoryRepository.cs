using Pokedex.Business.Core.Interfaces;
using Pokedex.Business.Entities;

namespace Pokedex.Business.Repositories
{
    public interface ICategoryRepository : IRepository
    {
        public Task<IEnumerable<Category>> GetAll();
    }
}