using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);

        Task<Category> GetWithBooksAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(int id, Category category);
        Task DeleteAsync(int id);
    }
}
