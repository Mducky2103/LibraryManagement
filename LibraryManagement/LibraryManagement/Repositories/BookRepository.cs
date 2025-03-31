using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books.Include(b => b.Category).Include(b => b.Author).ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books.Include(b => b.Category).Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Book book)
        {
            var bookFind = await _context.Books.FindAsync(id);

            if (bookFind != null)
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Book>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllAsync();

            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.Name.Contains(searchTerm) ||
                            b.Description.Contains(searchTerm) ||
                            b.Author.Name.Contains(searchTerm) ||
                            b.Category.Name.Contains(searchTerm))
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Author)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == authorId);
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }

}