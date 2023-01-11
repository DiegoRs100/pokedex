using Pokedex.Business.Core.Auth;

namespace Pokedex.Business.Services
{
    public interface IAuthenticationService
    {
        Task<string?> Login(Login request);
    }
}