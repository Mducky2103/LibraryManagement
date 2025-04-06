using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories.Interface;
using LibraryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class PenaltyRepository : IPenaltyRepository
    {
        private readonly LibraryDbContext _context;

        public PenaltyRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task AddPenaltyAsync(Penalty penalty)
        {
            if (penalty == null)
            {
                throw new ArgumentNullException(nameof(penalty), "Penalty object is null before adding to database.");
            }

            _context.Penalties.Add(penalty);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<PenaltyVm>> GetAllPenaltiesAsync()
        {
            return await _context.Penalties
                .Include(p => p.User)
                .Include(p => p.BorrowReceiptDetail)
                    .ThenInclude(brd => brd.Books) 
                .Select(p => new PenaltyVm
                {
                    Id = p.Id,
                    Username = p.User.UserName,
                    BorrowReceiptDetailId = p.BorrowReceiptDetailId,
                    BookName = p.BorrowReceiptDetail.Books.Name,
                    Amount = p.Amount,
                    Reason = p.Reason,
                    IssuedDate = p.IssuedDate,
                    Status = p.Status.ToString()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<PenaltyVm>> GetPenaltiesByUserIdAsync(string userId)
        {
            return await _context.Penalties
                .Include(p => p.User)
                .Include(p => p.BorrowReceiptDetail)
                    .ThenInclude(brd => brd.Books)
                .Where(p => p.UserId == userId)
                .Select(p => new PenaltyVm
                {
                    Id = p.Id,
                    Username = p.User.UserName,
                    BorrowReceiptDetailId = p.BorrowReceiptDetailId,
                    BookName = p.BorrowReceiptDetail.Books.Name,
                    Amount = p.Amount,
                    Reason = p.Reason,
                    IssuedDate = p.IssuedDate,
                    Status = p.Status.ToString()
                })
                .ToListAsync();
        }


    }
}
