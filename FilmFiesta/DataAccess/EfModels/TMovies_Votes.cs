namespace FilmFiesta.DataAccess.EfModels;

public class TMovies_Votes
{
    public long Id { get; set; }

    public long Iduser { get; set; }

    public long Idmovie { get; set; }

    public virtual TMovies IdmovieNavigation { get; set; }

    public virtual TUsers IduserNavigation { get; set; }
}
