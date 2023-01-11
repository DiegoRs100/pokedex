using Microsoft.Extensions.Options;
using Pokedex.Business.Core.Auth;
using Pokedex.Business.Core.Notifications;
using Pokedex.Business.Repositories;

namespace Pokedex.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public readonly INotifier _notifier;
        public readonly IUserRepository _userRepository;

        private readonly AuthSettings _authSettings;

        public AuthenticationService(IUserRepository userRepository,
                                     IOptions<AuthSettings> authSettings,
                                     INotifier notifier)
        {
            _userRepository = userRepository;
            _notifier = notifier;
            _authSettings = authSettings.Value;
        }

        public async Task<string?> Login(Login request)
        {
            var user = await _userRepository.GetByLogin(request.Username);

            if (user == null || user.Password != CryptographyHelper.EncryptToMD5(request.Password))
            {
                _notifier.Notify("Usuário ou senha incorretos.");
                return null;
            }

            var loginResponse = JwtHelper.GenerateTokenJwt(user!.Username, _authSettings);

            return loginResponse;
        }
    }
}