using System;
using System.Collections.Generic;

namespace FilmFiesta.Models
{
    public class MovieDetail
    {
        public long Id { get; set; }
        public string Titre { get; set; }
        public string Realisateur { get; set; }
        public string Description { get; set; }
        public string EN_Description { get; set; }
        public decimal? Duree { get; set; }
        public DateOnly? Date_de_sortie { get; set; }
        public string Video { get; set; }
        public string Affiche { get; set; }
        public List<string> Genres { get; set; }
        public string Statut { get; set; }
    }
}
