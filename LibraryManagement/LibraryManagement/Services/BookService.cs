using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<BookVm>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();

            return books.Select(b => new BookVm
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                YearPublished = b.YearPublished,
                Price = b.Price,
                Quantity = b.Quantity,
                Image = b.Image,
                IsAvailable = b.IsAvailable,
                CategoryName = b.Category?.Name,
                AuthorName = b.Author?.Name
            }).ToList();
        }

        public async Task<BookVm> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null) return null;

            return new BookVm
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                YearPublished = book.YearPublished,
                Price = book.Price,
                Quantity = book.Quantity,
                Image = book.Image,
                IsAvailable = book.IsAvailable,
                CategoryId = book.CategoryId,
                AuthorId = book.AuthorId
            };
        }

        public async Task AddBookAsync(BookAddVm bookVm)
        {
            var book = new Book
            {
                Name = bookVm.Name,
                Description = bookVm.Description,
                YearPublished = bookVm.YearPublished,
                Price = bookVm.Price,
                Quantity = bookVm.Quantity,
                Image = bookVm.Image,
                IsAvailable = bookVm.IsAvailable,
                CategoryId = bookVm.CategoryId,
                AuthorId = bookVm.AuthorId
            };

            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(int id, BookAddVm bookVm)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                throw new Exception("Not found book.");
            }

            book.Name = bookVm.Name;
            book.Description = bookVm.Description;
            book.YearPublished = bookVm.YearPublished;
            book.Price = bookVm.Price;
            book.Quantity = bookVm.Quantity;
            book.Image = bookVm.Image;
            book.IsAvailable = bookVm.IsAvailable;
            book.CategoryId = bookVm.CategoryId;
            book.AuthorId = bookVm.AuthorId;

            await _bookRepository.UpdateAsync(id, book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }
        public async Task<IEnumerable<BookVm>> SearchBooksAsync(string searchTerm)
        {
            var books = await _bookRepository.SearchAsync(searchTerm);
            return books.Select(b => new BookVm
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                YearPublished = b.YearPublished,
                Price = b.Price,
                Quantity = b.Quantity,
                Image = b.Image,
                IsAvailable = b.IsAvailable,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name
            });
        }

        public async Task<IEnumerable<BookVm>> GetBooksByCategoryAsync(int categoryId)
        {
            var books = await _bookRepository.GetByCategoryAsync(categoryId);
            return books.Select(b => new BookVm
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                YearPublished = b.YearPublished,
                Price = b.Price,
                Quantity = b.Quantity,
                Image = b.Image,
                IsAvailable = b.IsAvailable,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name
            });
        }

        public async Task<IEnumerable<BookVm>> GetBooksByAuthorAsync(int authorId)
        {
            var books = await _bookRepository.GetByAuthorAsync(authorId);
            return books.Select(b => new BookVm
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                YearPublished = b.YearPublished,
                Price = b.Price,
                Quantity = b.Quantity,
                Image = b.Image,
                IsAvailable = b.IsAvailable,
                CategoryId = b.CategoryId,
                CategoryName = b.Category?.Name,
                AuthorId = b.AuthorId,
                AuthorName = b.Author?.Name
            });
        }

    }
}
