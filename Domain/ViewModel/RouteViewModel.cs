using YolaGuide.Domain.Entity;

namespace YolaGuide.Domain.ViewModel
{
    public class RouteViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string Telephone { get; set; }

        public List<Place> Places { get; set; } = new();
    }
}
