using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Category> GetFirstCategories()
        {
            return _dbContext.Set<Category>().Where(category => category.Subcategory == null).ToList();
        } 

        public List<Category> GetSubcategories(Category mainCategory)
        {
            return _dbContext.Set<Category>().Where(category => category.Subcategory == mainCategory).ToList();
        }

        public Category GetCategoryByName(string name)
        {
            return _dbContext.Set<Category>().FirstOrDefault(category => category.Name == name);
        }
    }
}
