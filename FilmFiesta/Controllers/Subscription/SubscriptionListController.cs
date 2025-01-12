using FilmFiesta.Business.Interfaces;
using FilmFiesta.Requests.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.Controllers.Subscription
{
    [ApiController]
    [Route("Subcription")]
    public class SubscriptionListController : Controller
    {
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;

        public SubscriptionListController(ISubscriptionsBusiness subscriptionsBusiness)
        {
            _subscriptionsBusiness = subscriptionsBusiness;
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <response code="200">Subscriptions successfully obtained</response>
        /// <response code="404">Subscriptions not found</response>
        /// <response code="500">Subscriptions unsuccessfully obtained</response>
        /// <returns>The subscriptions</returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetAllSubscriptions([FromQuery] SubscriptionListRequest request)
        {
            try
            {
                IEnumerable<Dbo.Subscription> subscriptions = request == null || request.StartDate == null || request.EndDate == null
                    ? _subscriptionsBusiness.GetAll()
                    : _subscriptionsBusiness.GetAllInInterval((DateTime)request.StartDate, (DateTime)request.EndDate);
                return subscriptions.Count() <= 0 ? StatusCode(404, "Subscriptions not found") : StatusCode(200, subscriptions);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
