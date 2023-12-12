using YolaGuide.Domain.Enums;

namespace YolaGuide.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public State State { get; set; }

        public StateAdd StateAdd { get; set; }

        public Language Language { get; set; }

        public List<Place> Places { get; set; } = new();

        public List<Route> Routes { get; set; } = new();

        public List<Category> Categories { get; set; } = new();
    }
}
