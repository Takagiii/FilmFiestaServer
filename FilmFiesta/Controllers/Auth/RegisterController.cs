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
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Auth
{
    [ApiController]
    [Route("Register")]
    public class RegisterController : Controller
    {
        private readonly IUsersBusiness _usersBusiness;
        private readonly JwtHandler _jwtHandler;
        private readonly IMapper _mapper;
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;

        public RegisterController(IUsersBusiness usersBusiness, JwtHandler jwtHandler, IMapper mapper, ISubscriptionsBusiness subscriptionsBusiness)
        {
            _usersBusiness = usersBusiness;
            _subscriptionsBusiness = subscriptionsBusiness;
            _jwtHandler = jwtHandler;
            _mapper = mapper;
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <response code="201">User successfully registered</response>
        /// <response code="400">User could not be registered</response>
        /// <returns>The response, with jwt token</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AuthRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request: User could not be registered");
            }

            SubscriptionType subscription = _mapper.Map<SubscriptionType>(request.Subscription);
            User user = await _usersBusiness.Add(request.Email, request.Password, subscription);

            if (user == null)
            {
                return BadRequest(new AuthResponse { ErrorMessage = "Error registering user" });
            }

            DateTime? userSubscriptionEndDate = _subscriptionsBusiness.GetWithUserId(user.Id)?.EndDate;

            JwtSecurityToken tokenOptions = _jwtHandler.GenerateTokenOptions(_jwtHandler.GetSigningCredentials(), _jwtHandler.GetClaims(user.Name));
            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return StatusCode(201, new AuthResponse
            {
                isAuthSuccessful = true,
                Token = token,
                subscriptionType = subscription,
                IdUser = user.Id,
                EndSubscription = userSubscriptionEndDate.HasValue ? userSubscriptionEndDate.Value : null
            });
        }
    }
}
