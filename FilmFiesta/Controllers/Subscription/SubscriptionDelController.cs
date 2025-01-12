using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Subscription
{
    [ApiController]
    [Route("Subscription")]
    public class SubscriptionDelController : Controller
    {
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;

        public SubscriptionDelController(ISubscriptionsBusiness subscriptionsBusiness)
        {
            _subscriptionsBusiness = subscriptionsBusiness;
        }

        /// <summary>
        /// Delete a subscription
        /// </summary>
        /// <response code="200">Subscription successfully deleted</response>
        /// <response code="400">Subscription deletion was not successful</response>
        /// <returns>The subscription.</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription([FromRoute] long id, [FromBody] long idUser)
        {
            try
            {
                bool success = await _subscriptionsBusiness.Delete(id, idUser);
                return success ? Ok(success) : BadRequest("Subscription deletion was not successful");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
