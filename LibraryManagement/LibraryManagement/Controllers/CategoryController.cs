using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet("get-category-with-books/{id}")]
        public async Task<IActionResult> GetCategoryWithBooks(int id)
        {
            var categoryWithBooks = await _categoryService.GetCategoryWithBooksAsync(id);
            if (categoryWithBooks == null)
            {
                return NotFound();
            }
            return Ok(categoryWithBooks);
        }

        [AllowAnonymous]
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryVm categoryVm)
        {
            await _categoryService.AddCategoryAsync(categoryVm);

            return Ok();
        }

        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryVm categoryVm)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, categoryVm);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [AllowAnonymous]
        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return Ok();
        }
    }
}
