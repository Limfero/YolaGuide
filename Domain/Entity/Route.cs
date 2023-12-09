namespace YolaGuide.Domain.Entity
{
    public class Route
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Telephone { get; set; }

        public List<Place> Places { get; set; } = new();

        public List<User> Users { get; set; } = new();
    }
}
