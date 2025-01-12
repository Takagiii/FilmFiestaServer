using System.Collections.Generic;

namespace FilmFiesta.DataAccess.EfModels
{
    public class TUsers
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public long IDSubscription { get; set; }
        public virtual TSubscriptions IdsubscriptionNavigation { get; set; }
        public virtual ICollection<TMovies_Votes> TMoviesVotes { get; set; } = [];
    }
}
