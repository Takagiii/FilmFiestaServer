using System;

namespace FilmFiesta.Requests.Subscriptions
{
    public class SubscriptionListRequest
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
