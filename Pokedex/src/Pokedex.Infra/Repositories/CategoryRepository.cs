using Microsoft.EntityFrameworkCore;
using Pokedex.Business.Core.Repositories;
using Pokedex.Business.Entities;
using Pokedex.Business.Queries;
using Pokedex.Business.Repositories;

namespace Pokedex.Infra.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(EFDbContext eFDbContext) : base(eFDbContext)
        { }

        public Task<IEnumerable<Category>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}