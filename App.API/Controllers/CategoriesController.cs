using App.Services.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Update;
using App.Services.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return CreateActionResult(await categoryService.GetAllListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            return CreateActionResult(await categoryService.GetByIdAsync(id));
        }

        [HttpGet("{id}/products")]
        public async Task<IActionResult> GetCategoryWithProducts(int id)
        {
            return CreateActionResult(await categoryService.GetCategoryWithProductsAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            return CreateActionResult(await categoryService.CreateAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest request)
        {
            return CreateActionResult(await categoryService.UpdateAsync(id, request));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return CreateActionResult(await categoryService.DeleteAsync(id));
        }

        [HttpGet("products")]

        public async Task<IActionResult> GetCategoriesWithProducts()
        {
            return CreateActionResult(await categoryService.GetCategoryWithProductsAsync());
            
        }
    }
}
