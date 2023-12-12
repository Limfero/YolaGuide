using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.ViewModel;
using YolaGuide.DAL.Repositories.Interfaces;

namespace YolaGuide.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IBaseResponse<List<Category>> GetCategores(Category category)
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

        public IBaseResponse<Category> GetCategoryByName(string name)
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

        public async Task<IBaseResponse<Category>> CreateCategory(CategoryViewModel model)
        {
            try
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Subcategory = model.Subcategory
                };

                var response = await _categoryRepository.CreateAsync(category);

                return new BaseResponse<Category>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch(Exception ex) 
            {
                return new BaseResponse<Category>()
                {
                    Description = $"[CategoryService.CreateCategory] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }   
        }
    }
}
