﻿using LibraryManagement.Models;

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
        Task<IEnumerable<object>> GetBorrowHistoryAsync(string userId);
        Task<IEnumerable<object>> GetAllBorrowBookHistoryAsync(string userId);
        Task<object> GetPendingBorrowRequestsAsync();
        Task<object> GetExtendRequestsAsync();
        Task<object> GetOverdueBooksListAsync();
        Task<object> GetAllBorrowedBookAsync();
        Task<bool> ReturnBookAsync(int receiptDetailId);
        Task<bool> ReturnBookAndApplyPenaltyAsync(int receiptDetailId);
        Task<IEnumerable<object>> GetOverdueBooksByUserAsync(string userId);

    }
}
