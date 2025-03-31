using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVm>> GetAllCategoriesAsync();
        Task<List<CategoryVm>> GetAllCategoriesAsync1();
        Task<CategoryVm> GetCategoryByIdAsync(int id);
        Task<CategoryWithBooksVm> GetCategoryWithBooksAsync(int id);
        Task AddCategoryAsync(CategoryVm categoryVm);
        Task UpdateCategoryAsync(int id,CategoryVm categoryVm);
        Task DeleteCategoryAsync(int id);
    }
}
