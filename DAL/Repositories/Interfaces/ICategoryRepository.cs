using YolaGuide.Domain.Entity;

namespace YolaGuide.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        List<Category> GetFirstCategories();

        List<Category> GetSubcategories(Category mainCategory);

        Category GetCategoryByName(string name);

        List<Category> GetAllSubcategories();
    }
}
