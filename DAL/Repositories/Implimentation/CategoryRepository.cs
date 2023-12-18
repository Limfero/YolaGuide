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
            var categoties = _dbContext.Categories
                .Include(category => category.Subcategory)
                .Include(category => category.Subcategories)
                .Include(category => category.Places)
                .AsSingleQuery();

            return categoties
                .Where(category => category.Subcategory == null).ToList();
        } 

        public List<Category> GetSubcategories(Category mainCategory)
        {
            var categoties = _dbContext.Categories
                .Include(category => category.Subcategory)
                .Include(category => category.Subcategories)
                .Include(category => category.Places)
                .AsSingleQuery();

            return categoties
                .Where(category => category.Subcategory == mainCategory).ToList();
        }

        public Category GetCategoryByName(string name)
        {
            return _dbContext.Categories
                .Include(category => category.Subcategory)
                .Include(category => category.Subcategories)
                .Include(category => category.Places)
                .AsSingleQuery()
                .FirstOrDefault(category => category.Name.Contains(name));
        }

        public Category GetCategoryById(int id)
        {
            return _dbContext.Categories
                .Include(category => category.Subcategory)
                .Include(category => category.Subcategories)
                .Include(category => category.Places)
                .AsSingleQuery()
                .FirstOrDefault(category => category.Id == id);
        }

        public List<Category> GetAllSubcategories()
        {
            var categoties = _dbContext.Categories
                .Include(category => category.Subcategory)
                .Include(category => category.Subcategories)
                .Include(category => category.Places)
                .AsSingleQuery();

            return categoties
                .Where(category => category.Subcategories.Count == 0)
                .ToList();
        }
    }
}
