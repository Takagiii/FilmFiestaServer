using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.Models;
using FilmFiesta.Requests.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Subscription
{
    [ApiController]
    [Route("Subscription")]
    public class SubscriptionAddController : Controller
    {
        private readonly ISubscriptionsBusiness _subscriptionsBusiness;
        private readonly IMapper _mapper;
        public SubscriptionAddController(ISubscriptionsBusiness subscriptionsBusiness, IMapper mapper)
        {
            _subscriptionsBusiness = subscriptionsBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a subscription
        /// </summary>
        /// <response code="200">Subscription successfully added</response>
        /// <response code="400">Subscription could not be added</response>
        /// <returns>The subscription.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionAddRequest request)
        {
            try
            {
                int subscriptionType = _mapper.Map<int>(request.Subscriptiontype);
                if (!typeof(SubscriptionType).IsEnumDefined(subscriptionType))
                {
                    return BadRequest("Subscription could not be added: invalid subscription type");
                }
                DateTime endDate = _subscriptionsBusiness.GetEndDate(request.StartDate.Value, subscriptionType);
                Dbo.Subscription newSubscription = new() { StartDate = request.StartDate.Value, EndDate = endDate };
                Dbo.Subscription subscription = await _subscriptionsBusiness.Add(newSubscription, request.Id);
                return subscription == null ? BadRequest("Subscription could not be added") : Ok(subscription);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
