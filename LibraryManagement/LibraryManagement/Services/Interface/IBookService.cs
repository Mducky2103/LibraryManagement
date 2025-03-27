using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookVm>> GetAllBooksAsync();
        Task<BookVm> GetBookByIdAsync(int id);
        Task AddBookAsync(BookAddVm bookVm);
        Task UpdateBookAsync(int id,BookVm bookVm);
        Task DeleteBookAsync(int id);
    }
}
