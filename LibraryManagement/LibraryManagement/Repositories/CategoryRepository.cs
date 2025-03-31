using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryDbContext _context;

        public CategoryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Category category)
        {
            var cateFind = await _context.Categories.FindAsync(id);

            if (cateFind != null)
            {
                _context.Categories.Update(category);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<Category> GetWithBooksAsync(int id)
        {
            return await _context.Categories
             .Include(c => c.Books)
             .ThenInclude(b => b.Author)
             .FirstOrDefaultAsync(c => c.Id ==  id);
        }

    }
}
