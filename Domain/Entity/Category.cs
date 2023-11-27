namespace YolaGuide.Domain.Entity
{
    public class Category
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public int IdSubcategory { get; set; }
        public Category Subcategories { get; set; }

        public List<Place> Places { get; set; } = new();
    }
}
