using System;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.EfModels
{
    public class TSubscriptions
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ICollection<TUsers> TUsers { get; set; } = [];
    }
}
