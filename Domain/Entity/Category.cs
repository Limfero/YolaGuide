namespace YolaGuide.Domain.Entity
{
    public class Category
    {
        public int  Id { get; set; }

        public string Name { get; set; }

        public Category? Subcategory { get; set; }

        public List<Category> Subcategories { get; set; } = new();

        public List<Place> Places { get; set; } = new();

        public List<User> Users { get; set; } = new();  
    }
}
