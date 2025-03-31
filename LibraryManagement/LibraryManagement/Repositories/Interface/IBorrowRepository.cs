using LibraryManagement.Models;

namespace LibraryManagement.Repositories.Interface
{
    public interface IBorrowRepository
    {
        Task<IEnumerable<BorrowReceipt>> GetAllReceiptsAsync();
        Task<BorrowReceipt> GetReceiptByIdAsync(int id);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId);
        Task<IEnumerable<BorrowReceiptDetail>> GetBorrowedBooksByUserAsync(string userId);
        Task AddBorrowRequestAsync(BorrowReceipt borrowReceipt);
        Task UpdateBorrowStatusAsync(int detailId, BorrowStatus status);
        Task<bool> IsBookAvailableAsync(int bookId, int requestedQuantity);
        Task<IEnumerable<BorrowReceiptDetail>> GetLoanHistoryAsync(string userId);
        Task<int> GetTotalBooksBorrowedByUserAsync(string userId);
    }
}
