using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<IEnumerable<Book>> SearchAsync(string searchTerm);
        Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
        Task<IEnumerable<Book>> GetByAuthorAsync(int authorId);
        Task AddAsync(Book book);
        Task UpdateAsync(int id,Book book);
        Task DeleteAsync(int id);
        Task<Author> GetAuthorByIdAsync(int authorId);
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
