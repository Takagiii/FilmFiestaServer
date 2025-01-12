namespace FilmFiesta.DataAccess.EfModels
{
    public class TMovies_Genres
    {
        public long Id { get; set; }
        public long Movie_ID { get; set; }
        public long Genre_ID { get; set; }
        public virtual TGenres Genre { get; set; }
        public virtual TMovies Movie { get; set; }
    }
}
