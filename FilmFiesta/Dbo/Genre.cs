namespace FilmFiesta.Dbo
{
    public partial class Genre : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
