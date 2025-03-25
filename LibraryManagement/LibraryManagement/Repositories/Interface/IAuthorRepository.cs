using LibraryManagement.Models;

namespace LibraryManagement.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task AddAsync(Author author);
        Task UpdateAsync(int id, Author author);
        Task DeleteAsync(int id);
    }
}
