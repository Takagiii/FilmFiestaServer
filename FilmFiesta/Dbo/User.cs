namespace FilmFiesta.Dbo
{
    public partial class User : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public long IDSubscription { get; set; }
    }
}
