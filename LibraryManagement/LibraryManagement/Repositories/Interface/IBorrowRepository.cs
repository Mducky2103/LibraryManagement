using LibraryManagement.Models;

namespace LibraryManagement.Repositories.Interface
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<object>> GetAllReceiptsAsync();
        Task<object> GetReceiptByIdAsync(int id);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptId2Async(int receiptId);
        Task<object> GetReceiptDetailsWithOverdueStatus(int receiptId);
        Task<object> GetAllPendingRequest();
        Task<object> GetAllPendingRequest2();
        Task<object> GetAllOverdueBook();
        Task<IEnumerable<object>> GetBorrowedBooksByUserAsync(string userId);
        Task AddBorrowRequestAsync(BorrowReceipt borrowReceipt);
        Task UpdateBorrowStatusAsync(int detailId, BorrowStatus status);
        Task UpdateBorrowStatusAsync2(int detailId, BorrowStatus status);
        Task<bool> IsBookAvailableAsync(int bookId, int requestedQuantity);
        Task<IEnumerable<object>> GetLoanHistoryAsync(string userId);
        Task<int> GetTotalBooksBorrowedByUserAsync(string userId);
        Task UpdateOverdueBooksAsync();
        Task<IEnumerable<object>> GetOverdueBooksByUserAsync(string userId);
    }
}
