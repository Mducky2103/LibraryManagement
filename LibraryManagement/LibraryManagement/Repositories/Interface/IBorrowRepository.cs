using LibraryManagement.Models;

namespace LibraryManagement.Repositories.Interface
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<object>> GetAllReceiptsAsync();
        Task<object> GetReceiptByIdAsync(int id);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptId2Async(int receiptId);
        Task<IEnumerable<BorrowReceiptDetail>> GetBorrowedBooksByUserAsync(string userId);
        Task AddBorrowRequestAsync(BorrowReceipt borrowReceipt);
        Task UpdateBorrowStatusAsync(int detailId, BorrowStatus status);
        Task<bool> IsBookAvailableAsync(int bookId, int requestedQuantity);
        Task<IEnumerable<BorrowReceiptDetail>> GetLoanHistoryAsync(string userId);
        Task<int> GetTotalBooksBorrowedByUserAsync(string userId);
        Task UpdateOverdueBooksAsync();
        Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksAsync();
        Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksByUserAsync(string userId);
    }
}
