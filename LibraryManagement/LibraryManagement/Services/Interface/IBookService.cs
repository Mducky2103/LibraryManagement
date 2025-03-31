using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookVm>> GetAllBooksAsync();
        Task<BookVm> GetBookByIdAsync(int id);
        Task<IEnumerable<BookVm>> SearchBooksAsync(string searchTerm);
        Task<IEnumerable<BookVm>> GetBooksByCategoryAsync(int categoryId);
        Task<IEnumerable<BookVm>> GetBooksByAuthorAsync(int authorId);
        Task AddBookAsync(BookAddVm bookVm);
        Task UpdateBookAsync(int id, BookAddVm bookVm);
        Task DeleteBookAsync(int id);
    }
}
