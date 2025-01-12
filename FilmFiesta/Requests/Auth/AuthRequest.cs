using System.ComponentModel.DataAnnotations;

namespace FilmFiesta.Requests.Auth
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public int Subscription { get; set; } = 0;
    }
}
