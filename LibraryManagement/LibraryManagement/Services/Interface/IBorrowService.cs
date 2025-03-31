using LibraryManagement.Models;

namespace LibraryManagement.Services.Interface
{
    public interface IBorrowService
    {
        Task<IEnumerable<BorrowReceipt>> GetAllReceiptsAsync();
        Task<BorrowReceipt> GetReceiptByIdAsync(int id);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId);
        Task<string> RequestBorrowAsync(string userId, List<int> bookIds);
        Task<string> RequestExtendDueDateAsync(int receiptDetailId, string notes);
        Task<IEnumerable<BorrowReceiptDetail>> GetBorrowHistoryAsync(string userId);
        Task<IEnumerable<BorrowReceiptDetail>> GetAllBorrowBookHistoryAsync(string userId);
        Task<bool> ReturnBookAsync(int receiptDetailId);
    }
}
