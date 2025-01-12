namespace FilmFiesta.Dbo;

public class Movie_Vote : IObjectWithId
{
    public long Id { get; set; }

    public long Idmovie { get; set; }

    public long Iduser { get; set; }
}
