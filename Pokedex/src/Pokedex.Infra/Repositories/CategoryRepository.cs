using Dapper;
using Pokedex.Business.Core.Repositories;
using Pokedex.Business.Entities;
using Pokedex.Business.Repositories;

namespace Pokedex.Infra.Repositories
{
    public class CategoryRepository : RepositoryBase, ICategoryRepository
    {
        public CategoryRepository(DapperDbContext dapperDbContext) : base(dapperDbContext)
        { }

        public Task<IEnumerable<Category>> GetAll()
        {
            var query = "SELECT * FROM CATEGORY";
            return DapperContext.QueryAsync<Category>(query);
        }
    }
}