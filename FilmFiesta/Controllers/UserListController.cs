using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using FilmFiesta.Models;
using System.Linq;

namespace FilmFiesta.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserListController : Controller
    {
        private readonly IUsersBusiness _usersBusiness;

        public UserListController(IUsersBusiness usersBusiness)
        {
            _usersBusiness = usersBusiness;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <response code="200">Users successfully obtained</response>
        /// <response code="404">Users not found</response>
        /// <response code="500">Users unsuccessfully obtained</response>
        /// <returns>The users.</returns>
        // [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                IEnumerable<UserDetail> userDetails = _usersBusiness.GetAllUserDetails();
                return userDetails.Count() <= 0 ? NotFound("Users not found") : Ok(userDetails);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
