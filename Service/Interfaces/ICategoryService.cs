using YolaGuide.Domain.Entity;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.ViewModel;

namespace YolaGuide.Service.Interfaces
{
    public interface ICategoryService
    {
        IBaseResponse<List<Category>> GetCategores(Category category);

        IBaseResponse<List<Category>> GetAllSubcategores();

        IBaseResponse<Category> GetCategoryByName(string name);

        IBaseResponse<Category> GetCategoryById(int id);

        Task<IBaseResponse<Category>> CreateCategoryAsync(CategoryViewModel model);

        Task<IBaseResponse<Category>> RemoveCategoryAsync(Category category);
    }
}
