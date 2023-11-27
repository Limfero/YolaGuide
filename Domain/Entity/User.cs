namespace YolaGuide.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public List<Place> Places { get; set; } = new();
    }
}
