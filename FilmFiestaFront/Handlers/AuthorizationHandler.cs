using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace FilmFiestaFront.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public AuthorizationHandler(IHttpContextAccessor accessor) { 
            _accessor = accessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddHeader(request);
            return await base.SendAsync(request, cancellationToken);
        }
         private void AddHeader(HttpRequestMessage request)
        {
            string? JWToken = _accessor?.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(JWToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWToken);
            }
        }
    }
}
