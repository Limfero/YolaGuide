using YolaGuide.DAL.Repositories.Implimentation;
using YolaGuide.DAL;
using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.Entity;

namespace YolaGuide.Service
{
    public static class CategoryService
    {
        private static readonly CategoryRepository _categoryRepository = new(new ApplicationDbContext(new()));

        public static IBaseResponse<List<Category>> GetCategores(Category category)
        {
            try
            {
                var response = new List<Category>();

                if (category != null)
                    response = _categoryRepository.GetSubcategories(category);
                else
                    response = _categoryRepository.GetFirstCategories();

                return new BaseResponse<List<Category>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Category>>()
                {
                    Description = $"[CategoryService.GetFirstCategory] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public static IBaseResponse<Category> GetCategoryByName(string name)
        {
            try
            {
                var response = _categoryRepository.GetCategoryByName(name);

                return new BaseResponse<Category>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<Category>()
                {
                    Description = $"[CategoryService.GetCategoryByName] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
