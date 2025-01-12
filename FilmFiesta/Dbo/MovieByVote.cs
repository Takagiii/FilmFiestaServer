namespace FilmFiesta.Dbo;

public partial class MovieByVote
{
    public string Titre { get; set; }

    public long Idmovie { get; set; }

    public int? VoteCount { get; set; }
}
