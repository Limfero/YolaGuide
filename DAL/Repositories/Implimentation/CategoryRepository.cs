using YolaGuide.DAL.Repositories.Implementation;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace YolaGuide.DAL.Repositories.Implimentation
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public List<Category> GetFirstCategories()
        {
            return _dbContext.Categories
                .Include(category => category.Subcategory)
                .AsSplitQuery()
                .Where(category => category.Subcategory == null).ToList();
        } 

        public List<Category> GetSubcategories(Category mainCategory)
        {
            return _dbContext.Categories
                .Include(category => category.Subcategory)
                .AsSplitQuery()
                .Where(category => category.Subcategory == mainCategory).ToList();
        }

        public Category GetCategoryByName(string name)
        {
            return _dbContext.Set<Category>().FirstOrDefault(category => category.Name == name);
        }

        public List<Category> GetAllSubcategories()
        {
            return _dbContext.Categories
                .Include(category => category.Subcategories)
                .AsSplitQuery()
                .Where(category => category.Subcategories.Count == 0).ToList();
        }
    }
}
