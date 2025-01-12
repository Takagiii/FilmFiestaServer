using System;

namespace FilmFiesta.Models.Auth
{
    public class AuthResponse
    {
        public bool isAuthSuccessful { get; set; } = false;
        public string ErrorMessage { get; set; } = null;
        public string Token { get; set; } = null;
        public SubscriptionType subscriptionType { get; set; } = SubscriptionType.Free;

        public long IdUser { get; set; }
        public DateTime? EndSubscription { get; set; }
    }
}
