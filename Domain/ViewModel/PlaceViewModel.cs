using YolaGuide.Domain.Entity;

namespace YolaGuide.Domain.ViewModel
{
    public class PlaceViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ContactInformation { get; set; }

        public string Image { get; set; }

        public long YIdOrganization { get; set; }

        public string Coordinates { get; set; }

        public List<Category> Categories { get; set; } = new();
    }
}
