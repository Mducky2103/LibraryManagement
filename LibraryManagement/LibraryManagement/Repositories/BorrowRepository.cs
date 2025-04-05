using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowRepository(LibraryDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả phiếu mượn
        public async Task<IEnumerable<object>> GetAllReceiptsAsync()
        {
            return await _context.BorrowReceipts
                    .Include(br => br.User)
                    .Include(br => br.Details)
                    .ThenInclude(d => d.Books)
                    .Select(br => new
                    {
                        br.Id,
                        UserEmail = br.User.Email,
                        br.TotalBooks,
                        Details = br.Details.Select(d => new
                        {
                            d.Id,
                            BookName = d.Books.Name,
                            d.BorrowedDate,
                            d.DueDate,
                            d.ReturnedDate,
                            d.Status,
                            d.Notes
                        })
                    })
                    .ToListAsync();
        }

        // Lấy thông tin phiếu mượn theo ID
        public async Task<object> GetReceiptByIdAsync(int id)
        {
            return await _context.BorrowReceipts
                .Include(br => br.User)
                .Where(br => br.Id == id)
                .Select(br => new
                {
                    br.Id,
                    UserName = br.User.UserName,
                    br.TotalBooks
                })
                .FirstOrDefaultAsync();
        }

        // Lấy danh sách chi tiết mượn sách theo BorrowReceiptId
        public async Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId)
        {
            return await _context.BorrowReceiptDetails
                .Where(d => d.Id == receiptId)
                .Include(d => d.Books)
                .ToListAsync();
        }

        // Lấy danh sách chi tiết mượn sách theo BorrowReceiptId (Over Due)
        public async Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptId2Async(int receiptId)
        {
            return await _context.BorrowReceiptDetails
                .Include(brd => brd.Books)
                .Include(brd => brd.BorrowReceipt)
                .Where(brd => brd.Id == receiptId)
                .ToListAsync();
        }

        // Thêm một yêu cầu mượn sách mới
        public async Task AddBorrowRequestAsync(BorrowReceipt borrowReceipt)
        {
            await _context.BorrowReceipts.AddAsync(borrowReceipt);
            await _context.SaveChangesAsync();
        }

        // Cập nhật trạng thái mượn sách (Pending -> Approved, Returned)
        public async Task UpdateBorrowStatusAsync(int detailId, BorrowStatus status)
        {
            var borrowDetail = await _context.BorrowReceiptDetails.FindAsync(detailId);
            var book = await _context.Books.FindAsync(borrowDetail.BookId);
            if (borrowDetail != null)
            {
                borrowDetail.Status = status;

                // Nếu trạng thái là Approved, cập nhật số lượng sách
                if (status == BorrowStatus.Approved)
                {
                    if (book != null)
                    {
                        book.Quantity -= 1;  // Giảm số lượng sách trong kho
                    }
                }
                else if (status == BorrowStatus.Returned)
                {
                    if (book != null)
                    {
                        book.Quantity += 1;  // Tăng số lượng sách trong kho
                        _context.Books.Update(book);  // Cập nhật số lượng sách khi trả sách
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
        //Cập nhật trạng thái mượn sách (Pending ->
        public async Task UpdateBorrowStatusAsync2(int detailId, BorrowStatus status)
        {
            var borrowDetail = await _context.BorrowReceiptDetails.FindAsync(detailId);
            var book = await _context.Books.FindAsync(borrowDetail.BookId);
            if (borrowDetail != null)
            {
                borrowDetail.Status = status;
                // Ko thay đổi số lượng sách trong kho do gia hạn sách

                await _context.SaveChangesAsync();
            }
        }

        // Kiểm tra xem sách có đủ số lượng để mượn không
        public async Task<bool> IsBookAvailableAsync(int bookId, int requestedQuantity)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            int borrowedBooks = await _context.BorrowReceiptDetails
                .Where(d => d.BookId == bookId && d.Status == BorrowStatus.Approved)
                .CountAsync();

            return book.Quantity - borrowedBooks >= requestedQuantity;
        }

        // Lấy tổng số sách mà một user đang mượn (chỉ tính trạng thái Approved)
        public async Task<int> GetTotalBooksBorrowedByUserAsync(string userId)
        {
            return await _context.BorrowReceipts
                .Where(br => br.UserId == userId)
                .SumAsync(br => br.TotalBooks);
        }

        // Lấy danh sách sách mà một user đang mượn
        public async Task<IEnumerable<object>> GetBorrowedBooksByUserAsync(string userId)
        {
            return await _context.BorrowReceiptDetails
                .Include(r => r.Books)
                .Where(r => r.BorrowReceipt.UserId == userId && r.Status == BorrowStatus.Approved)
                .Select(r => new
                {
                    r.Id,
                    BookName = r.Books.Name,
                    r.BorrowedDate,
                    r.DueDate,
                    r.ReturnedDate,
                    r.Status,
                    r.Notes
                })
                .ToListAsync();
        }

        // Lấy lịch sử mượn sách của một user
        public async Task<IEnumerable<object>> GetLoanHistoryAsync(string userId)
        {
            return await _context.BorrowReceiptDetails
                .Include(r => r.Books)
                .Where(r => r.BorrowReceipt.UserId == userId)
                .Select(r => new
                {
                    r.Id,
                    BookName = r.Books.Name, 
                    r.BorrowedDate,
                    r.DueDate,
                    r.ReturnedDate,
                    r.Status,
                    r.Notes
                })
                .ToListAsync();
        }

        //Tự động update những quyển sách đang mượn quá thời hạn trả sách
        public async Task UpdateOverdueBooksAsync()
        {
            var overdueBooks = await _context.BorrowReceiptDetails
                            .Where(d => d.Status == BorrowStatus.Approved && d.DueDate < DateTime.Now)
                            .ToListAsync();

            foreach (var detail in overdueBooks)
            {
                detail.Status = BorrowStatus.Overdue;
            }

            if (overdueBooks.Any())
            {
                await _context.SaveChangesAsync();
            }
        }

        // Lấy danh sách sách mượn quá hạn
        public async Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksAsync()
        {
            return await _context.BorrowReceiptDetails
                .Where(d => d.Status == BorrowStatus.Overdue)
                .ToListAsync();
        }

        // Lấy danh sách sách mượn quá hạn của một user
        public async Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksByUserAsync(string userId)
        {
            return await _context.BorrowReceiptDetails
                .Where(d => d.BorrowReceipt.UserId == userId && d.Status == BorrowStatus.Overdue)
                .ToListAsync();
        }
    }
}
