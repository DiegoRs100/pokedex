using Pokedex.Business.Core.Auth;
using Pokedex.Business.Repositories;

namespace Pokedex.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User> GetByLogin(string login)
        {
            // Esse dado foi mocado para simplificação do exemplo,
            // uma vez que o acesso ao banco de dados já foi tratado em outros repositórios.
            return Task.FromResult(new User()
            {
                Username = "Ash@devpack.com.br",
                Password = "81dc9bdb52d04dc20036dbd8313ed055" // Password = 1234
            });
        }
    }
}