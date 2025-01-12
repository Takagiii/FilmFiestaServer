using System;

namespace FilmFiesta.Dbo
{
    public partial class Subscription : IObjectWithId
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
