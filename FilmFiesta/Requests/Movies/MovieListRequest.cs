using System.Collections.Generic;

namespace FilmFiesta.Requests.Movies
{
    public class MovieListRequest
    {
        public List<string> Genres { get; set; } = [];

        public string Research { get; set; } = "";

        public List<string> Status { get; set; } = ["Paid", "Free", "Voted"];
    }
}
