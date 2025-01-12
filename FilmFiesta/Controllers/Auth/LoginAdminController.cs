using FilmFiesta.Business.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models.Auth;
using FilmFiesta.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FilmFiesta.Controllers.Auth
{
    [ApiController]
    [Route("Login")]
    public class LoginAdminController : Controller
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUsersBusiness _usersBusiness;

        public LoginAdminController(JwtHandler jwtHandler, IUsersBusiness usersBusiness)
        {
            _jwtHandler = jwtHandler;
            _usersBusiness = usersBusiness;
        }

        /// <summary>
        /// Log in the admin user
        /// </summary>
        /// <response code="200">User admin successfully login</response>
        /// <response code="400">User admin could not be logged in</response>
        /// <returns>The response, with success and token</returns>
        [AllowAnonymous]
        [HttpPost("Admin")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            User user = _usersBusiness.GetByLogin(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new AuthResponse { ErrorMessage = "Invalid Authentification" });
            }

            if (!user.Role.Contains("Admin"))
            {
                return Unauthorized(new AuthResponse { ErrorMessage = "User not admin" });
            }

            JwtSecurityToken tokenOptions = _jwtHandler.GenerateTokenOptions(_jwtHandler.GetSigningCredentials(), _jwtHandler.GetClaims(user.Name));
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthResponse
            {
                isAuthSuccessful = true,
                Token = token,
            });
        }
    }
}
