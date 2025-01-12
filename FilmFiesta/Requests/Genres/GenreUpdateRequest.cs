using System.ComponentModel.DataAnnotations;

namespace FilmFiesta.Requests.Genres
{
    public class GenreUpdateRequest
    {
        [Required]
        public long genreId { get; set; }
        public string genreName { get; set; }
    }
}
