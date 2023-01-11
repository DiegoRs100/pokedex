using Microsoft.AspNetCore.Mvc;
using Pokedex.Business.Core.Auth;
using Pokedex.Business.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace Pokedex.Api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [SwaggerOperation("Realizar login.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(Login login)
        {
            var token = await _authenticationService.Login(login);
            return Ok(token);
        }
    }
}