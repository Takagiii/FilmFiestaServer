namespace FilmFiesta.Dbo
{
    public partial class Movie_Genre : IObjectWithId
    {
        public long Id { get; set; }
        public long Movie_ID { get; set; }
        public long Genre_ID { get; set; }
    }
}
