using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmFiesta.Requests.Movies
{
    public class MovieAddRequest
    {
        [Required]
        public string Titre { get; set; }
        public string Realisateur { get; set; }
        public string Description { get; set; }
        public string EN_Description { get; set; }
        public decimal? Duree { get; set; }
        public DateOnly? Date_de_sortie { get; set; }
        public string Video { get; set; }
        public string Affiche { get; set; }
        public string Statut { get; set; } = "Paid";
        public List<string> Genres { get; set; }
    }
}
