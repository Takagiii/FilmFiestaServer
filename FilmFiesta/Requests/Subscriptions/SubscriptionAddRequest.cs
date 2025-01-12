using System;

namespace FilmFiesta.Requests.Subscriptions
{
    public class SubscriptionAddRequest
    {
        public long Id { get; set; }
        public DateTime? StartDate { get; set; } = DateTime.Today;
        public string Subscriptiontype { get; set; }
    }
}
