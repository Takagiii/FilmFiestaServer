using FilmFiesta.Models;
using System;

namespace FilmFiesta.Dbo
{
    public partial class Movie : IObjectWithId
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
        public StatutType Statut { get; set; }
    }
}
