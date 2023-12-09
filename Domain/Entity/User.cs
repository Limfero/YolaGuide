namespace YolaGuide.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public List<Place> Places { get; set; } = new();

        public List<Route> Routes { get; set; } = new();
    }
}
