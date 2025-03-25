using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorVm>> GetAllAuthorsAsync();
        Task<AuthorVm> GetAuthorByIdAsync(int id);
        Task AddAuthorAsync(AuthorVm authorVm);
        Task UpdateAuthorAsync(int id, AuthorVm authorVm);
        Task DeleteAuthorAsync(int id);
    }
}
