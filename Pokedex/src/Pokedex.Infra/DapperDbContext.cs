using Microsoft.Data.SqlClient;

namespace Pokedex.Infra
{
    public class DapperDbContext
    {
        public SqlConnection Context { get; set; }

        public DapperDbContext(string connectionString)
        {
            Context = new SqlConnection(connectionString);
        }
    }
}