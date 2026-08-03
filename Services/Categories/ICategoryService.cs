using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using App.Services.Dto;

namespace App.Services.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResult<List<CategoryDto>>> GetAllListAsync();

        Task<ServiceResult<CategoryDto>> GetByIdAsync(int id);

        Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId);

        Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync();
        
        Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request);

        Task<ServiceResult> UpdateAsync(int Id,UpdateCategoryRequest request);

        Task<ServiceResult> DeleteAsync(int id);
    }
}
