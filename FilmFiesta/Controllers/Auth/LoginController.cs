using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using FilmFiesta.Models.Auth;
using FilmFiesta.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FilmFiesta.Controllers.Auth
{
    [ApiController]
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly JwtHandler _jwtHandler;
        private readonly IUsersBusiness _usersBusiness;
        private readonly IMapper _mapper;
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;

        public LoginController(JwtHandler jwtHandler, IUsersBusiness usersBusiness, IMapper mapper, ISubscriptionsBusiness subscriptionsBusiness)
        {
            _jwtHandler = jwtHandler;
            _usersBusiness = usersBusiness;
            _mapper = mapper;
            _subscriptionsBusiness = subscriptionsBusiness;
        }

        /// <summary>
        /// Log in the user
        /// </summary>
        /// <response code="200">User successfully login</response>
        /// <response code="400">User could not be logged in</response>
        /// <returns>The response, with success and token</returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            User user = _usersBusiness.GetByLogin(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new AuthResponse { ErrorMessage = "Invalid Authentification" });
            }

            DateTime? userSubscriptionEndDate = _subscriptionsBusiness.GetWithUserId(user.Id)?.EndDate;

            JwtSecurityToken tokenOptions = _jwtHandler.GenerateTokenOptions(_jwtHandler.GetSigningCredentials(), _jwtHandler.GetClaims(user.Name));
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthResponse
            {
                isAuthSuccessful = true,
                Token = token,
                IdUser = user.Id,
                subscriptionType = userSubscriptionEndDate.HasValue ? SubscriptionType.Month : SubscriptionType.Free,
                EndSubscription = userSubscriptionEndDate.HasValue ? userSubscriptionEndDate.Value : null
            });
        }
    }
}
