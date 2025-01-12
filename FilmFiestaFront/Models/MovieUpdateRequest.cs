using FilmFiestaFront.Validators;
using System.ComponentModel.DataAnnotations;

namespace FilmFiestaFront.Models
{
    public class MovieUpdateRequest
    {
        [Required]
        public long Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Titre { get; set; }
        public string? Realisateur { get; set; }
        public string? Description { get; set; }
        public string? EN_Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public decimal? Duree { get; set; }

        [NotInFuture]
        public DateOnly? Date_de_sortie { get; set; }

        [Required(ErrorMessage = "Video URL is required")]
        [Url]
        public string Video { get; set; }

        [Required(ErrorMessage = "Poster image URL is required")]
        [Url]
        public string Affiche { get; set; }

        [Required(ErrorMessage = "Statut is required")]
        public string Statut { get; set; }

        [Required(ErrorMessage = "You need to choose at least 1 genre")]
        public IEnumerable<string> Genres { get; set; }
    }
}
