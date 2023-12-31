﻿using YolaGuide.Domain.Enums;
using YolaGuide.Domain.Response;
using YolaGuide.Domain.Entity;
using YolaGuide.Domain.ViewModel;
using YolaGuide.DAL.Repositories.Interfaces;
using YolaGuide.Service.Interfaces;

namespace YolaGuide.Service.Implementation
{
    public class CategoryService : ICategoryService
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

        public IBaseResponse<List<Category>> GetAllSubcategores()
        {
            try
            {
                var response = _categoryRepository.GetAllSubcategories();

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

        public IBaseResponse<Category> GetCategoryById(int id)
        {
            try
            {
                var response = _categoryRepository.GetCategoryById(id);

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

        public async Task<IBaseResponse<Category>> CreateCategoryAsync(CategoryViewModel model)
        {
            try
            {
                var category = new Category()
                {
                    Name = model.Name,
                    Subcategory = model.Subcategory
                };

                await _categoryRepository.CreateAsync(category);

                return new BaseResponse<Category>()
                {
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Category>()
                {
                    Description = $"[CategoryService.CreateCategory] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<Category>> RemoveCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.RemoveAsync(category);

                return new BaseResponse<Category>()
                {
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
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
