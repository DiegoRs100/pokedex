using Microsoft.Data.SqlClient;
using Pokedex.Business.Core.Interfaces;
using Pokedex.Infra;

namespace Pokedex.Business.Core.Repositories
{
    public class RepositoryBase : IRepository
    {
        protected readonly EFDbContext EFDbContext = default!;
        protected readonly SqlConnection DapperContext = default!;

        protected RepositoryBase(EFDbContext eFDbContext, DapperDbContext dapperContext)
        {
            EFDbContext = eFDbContext;
            DapperContext = dapperContext.Context;
        }

        protected RepositoryBase(DapperDbContext dapperContext)
        {
            DapperContext = dapperContext.Context;
        }

        protected RepositoryBase(EFDbContext eFDbContext)
        {
            EFDbContext = eFDbContext;
        }

        public Task SaveChanges()
        {
            return EFDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                EFDbContext?.Dispose();
                DapperContext?.Dispose();
            }
        }
    }
}