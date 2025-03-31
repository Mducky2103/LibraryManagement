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
        public async Task<IEnumerable<BorrowReceipt>> GetAllReceiptsAsync()
        {
            return await _context.BorrowReceipts
                .Include(br => br.User)
                .Include(br => br.Details)
                .ThenInclude(d => d.Books)
                .ToListAsync();
        }

        // Lấy thông tin phiếu mượn theo ID
        public async Task<BorrowReceipt> GetReceiptByIdAsync(int id)
        {
            return await _context.BorrowReceipts
                .Include(br => br.User)
                .Include(br => br.Details)
                .ThenInclude(d => d.Books)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        // Lấy danh sách chi tiết mượn sách theo BorrowReceiptId
        public async Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId)
        {
            return await _context.BorrowReceiptDetails
                .Where(d => d.Id == receiptId)
                .Include(d => d.Books)
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
        public async Task<IEnumerable<BorrowReceiptDetail>> GetBorrowedBooksByUserAsync(string userId)
        {
            return await _context.BorrowReceiptDetails
                .Where(r => r.BorrowReceipt.UserId == userId && r.Status == BorrowStatus.Approved)
                .ToListAsync();
        }

        // Lấy lịch sử mượn sách của một user
        public async Task<IEnumerable<BorrowReceiptDetail>> GetLoanHistoryAsync(string userId)
        {
            return await _context.BorrowReceiptDetails
                .Where(r => r.BorrowReceipt.UserId == userId)
                .ToListAsync();
        }
    }
}
