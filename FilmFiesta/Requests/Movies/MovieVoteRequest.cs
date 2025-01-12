namespace FilmFiesta.Requests.Movies
{
    public class MovieVoteRequest
    {
        public long UserId { get; set; }
        public long MovieId { get; set; }
    }
}
