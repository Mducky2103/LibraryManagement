using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryVm>> GetAllCategoriesAsync()
        {
            var category = await _categoryRepository.GetAllAsync();

            return category.Select(c => new CategoryVm
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<CategoryVm> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null) return null;

            return new CategoryVm
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task AddCategoryAsync(CategoryVm categoryVm)
        {
            var category = new Category
            {
                Name = categoryVm.Name
            };

            await _categoryRepository.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryVm categoryVm)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null) throw new Exception("Not found category.");

            category.Name = categoryVm.Name;

            await _categoryRepository.UpdateAsync(id, category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
        }
    }
}
