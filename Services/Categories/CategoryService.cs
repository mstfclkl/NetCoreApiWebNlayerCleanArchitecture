using App.Repositories;
using App.Repositories.Categories;
using App.Repositories.Products;
using App.Services.Categories.Create;
using App.Services.Products.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App.Services.Categories.Update;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using App.Services.Dto;


namespace App.Services.Categories
{
    public class CategoryService(ICategoryRepository categoryRepository,IUnitOfWork unitOfWork, IMapper mapper) : ICategoryService
    {


        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await categoryRepository.GetAll().ToListAsync();
            var categoriesAsDto = mapper.Map<List<CategoryDto>>(categories);
            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ServiceResult<CategoryDto>.Fail("Category not found.", HttpStatusCode.NotFound);
            }
            var categoryAsDto = mapper.Map<CategoryDto>(category);
            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var category = await categoryRepository.GetCategoryWithProductsAsync(categoryId);
            if (category is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Category not found.", HttpStatusCode.NotFound);
            }
            var categoriesAsDto = mapper.Map<CategoryWithProductsDto>(category);
            return ServiceResult<CategoryWithProductsDto>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
        {
            var categories = await categoryRepository.GetCategoryWithProductsAsync().ToListAsync();
            
            var categoriesAsDto = mapper.Map<List<CategoryWithProductsDto>>(categories);
            
            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {

            var anyCategory = await categoryRepository.Where(c => c.Name == request.Name).AnyAsync();

            if (anyCategory)
            {
                return ServiceResult<int>.Fail("Category name already exists.", HttpStatusCode.NotFound);
            }

            
            var newCategory = mapper.Map<Category>(request);

            await categoryRepository.AddAsync(newCategory);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult<int>.SuccessAsCreated(newCategory.Id,$"/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync (int id, UpdateCategoryRequest request)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ServiceResult.Fail("Category not found.", HttpStatusCode.NotFound);
            }
           
            var isCategoryNameExist = await categoryRepository.Where(c => c.Name == request.Name && c.Id != id).AnyAsync();
            if (isCategoryNameExist)
            {
                return ServiceResult.Fail("Category name already exists.", HttpStatusCode.BadRequest);
            }
            category = mapper.Map(request, category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return ServiceResult.Fail("Category not found.", HttpStatusCode.NotFound);
            }
            categoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

    }
}
