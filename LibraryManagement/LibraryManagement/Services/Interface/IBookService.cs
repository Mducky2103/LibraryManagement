using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookVm>> GetAllBooksAsync();
        Task<BookVm> GetBookByIdAsync(int id);
        Task AddBookAsync(BookAddVm bookVm);
        Task UpdateBookAsync(int id, BookAddVm bookVm);
        Task DeleteBookAsync(int id);
    }
}
