using LibraryManagement.Models;

namespace LibraryManagement.Services.Interface
{
    public interface IBorrowService
    {
        Task<IEnumerable<object>> GetAllReceiptsAsync();
        Task<object> GetReceiptByIdAsync(int id);
        Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId);
        Task<string> RequestBorrowAsync(string userId, List<int> bookIds);
        Task<bool> ApproveBorrowRequestAsync(int receiptDetailId);
        Task<bool> CancelBorrowRequestAsync(int receiptDetailId, string notes);
        Task<string> RequestExtendDueDateAsync(int receiptDetailId, string notes);
        Task<bool> ProcessExtendDueDateRequestAsync(int receiptDetailId, bool isApproved, string notes);
        Task<IEnumerable<BorrowReceiptDetail>> GetBorrowHistoryAsync(string userId);
        Task<IEnumerable<BorrowReceiptDetail>> GetAllBorrowBookHistoryAsync(string userId);
        Task<bool> ReturnBookAsync(int receiptDetailId);
        Task<bool> ReturnBookAndApplyPenaltyAsync(int receiptDetailId);
        Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksAsync();
        Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksByUserAsync(string userId);

    }
}
