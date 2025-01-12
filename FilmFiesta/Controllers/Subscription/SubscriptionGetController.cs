using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmFiesta.Controllers.Subscription
{
    [ApiController]
    [Route("Subscription")]
    public class SubscriptionGetController : Controller
    {
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;

        public SubscriptionGetController(ISubscriptionsBusiness subscriptionsBusiness)
        {
            _subscriptionsBusiness = subscriptionsBusiness;
        }

        /// <summary>
        /// Gets the subscription
        /// </summary>
        /// <remarks>
        /// An id is needed
        /// </remarks>
        /// <response code="200">Subscription successfully get</response>
        /// <response code="400">Bad request: the id cannot be negative or null</response>
        /// <response code="404">Subcription not found</response>
        /// <response code="500">Subcription unsuccessfully get</response>
        /// <returns>The subscription.</returns>
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetSubcription(long id)
        {
            Dbo.Subscription subscription = _subscriptionsBusiness.Get(id);
            return subscription == null ? NotFound("Subscription not found") : Ok(subscription);
        }
    }
}
