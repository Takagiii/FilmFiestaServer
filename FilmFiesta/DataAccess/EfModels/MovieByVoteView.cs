namespace FilmFiesta.DataAccess.EfModels;

public partial class MovieByVoteView
{
    public string Titre { get; set; }

    public long Idmovie { get; set; }

    public int? VoteCount { get; set; }
}
