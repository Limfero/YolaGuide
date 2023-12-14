namespace YolaGuide.Domain.Entity
{
    public class Place
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ContactInformation { get; set; }

        public string Adress { get; set; }

        public string Image { get; set; }

        public long YIdOrganization { get; set; }

        public string Coordinates { get; set; }

        public List<Category> Categories { get; set; } = new();

        public List<User> Users { get; set; } = new();

        public List<Route> Routes { get; set; } = new();
    }
}
