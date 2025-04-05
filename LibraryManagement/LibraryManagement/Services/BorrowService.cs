using LibraryManagement.Repositories.Interface;
using LibraryManagement.Repositories;
using LibraryManagement.Services.Interface;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;
        private readonly IPenaltyRepository _penaltyRepository;

        public BorrowService(IBorrowRepository borrowRepository, IPenaltyRepository penaltyRepository)
        {
            _borrowRepository = borrowRepository;
            _penaltyRepository = penaltyRepository;
        }
        // Lấy tất cả phiếu mượn
        public async Task<IEnumerable<object>> GetAllReceiptsAsync()
        {
            return await _borrowRepository.GetAllReceiptsAsync();
        }

        // Lấy thông tin một phiếu mượn theo ID
        public async Task<object> GetReceiptByIdAsync(int id)
        {
            return await _borrowRepository.GetReceiptByIdAsync(id);
        }

        // Lấy chi tiết phiếu mượn theo ID phiếu mượn
        public async Task<IEnumerable<BorrowReceiptDetail>> GetReceiptDetailsByReceiptIdAsync(int receiptId)
        {
            return await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptId);
        }

        // Thành viên gửi yêu cầu mượn sách
        public async Task<string> RequestBorrowAsync(string userId, List<int> bookIds)
        {
            if (bookIds == null || !bookIds.Any())
                return "Danh sách sách mượn không hợp lệ.";

            var borrowReceipt = new BorrowReceipt
            {
                UserId = userId,
                BorrowDate = null, // Chưa được duyệt
                DueDate = null,
                TotalBooks = bookIds.Count,
                Details = new List<BorrowReceiptDetail>()
            };

            foreach (var bookId in bookIds)
            {
                // Kiểm tra sách có sẵn không
                bool isAvailable = await _borrowRepository.IsBookAvailableAsync(bookId, 1);
                if (!isAvailable)
                    return $"Sách ID {bookId} hiện không có sẵn.";

                borrowReceipt.Details.Add(new BorrowReceiptDetail
                {
                    BookId = bookId,
                    Status = BorrowStatus.Pending,
                    BorrowedDate = null,
                    DueDate = null
                });
            }

            await _borrowRepository.AddBorrowRequestAsync(borrowReceipt);
            return "Yêu cầu mượn sách đã được gửi.";
        }

        // Thủ thư phê duyệt yêu cầu mượn sách
        public async Task<bool> ApproveBorrowRequestAsync(int receiptDetailId)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptDetailId);
            var receiptDetail = receiptDetails.FirstOrDefault(rd => rd.Id == receiptDetailId);
            if (receiptDetail == null)
            {
                Console.WriteLine($"Không tìm thấy chi tiết phiếu mượn với ID: {receiptDetailId}");
                return false;
            }

            if (receiptDetail.Status != BorrowStatus.Pending)
            {
                Console.WriteLine($"Trạng thái phiếu mượn không hợp lệ: {receiptDetail.Status}");
                return false;
            }

            receiptDetail.BorrowedDate = DateTime.Now;
            receiptDetail.DueDate = DateTime.Now.AddDays(30);
            await _borrowRepository.UpdateBorrowStatusAsync(receiptDetailId, BorrowStatus.Approved);

            return true;
        }

        // Thủ thư từ chối yêu cầu mượn sách
        public async Task<bool> CancelBorrowRequestAsync(int receiptDetailId, string notes)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptDetailId);
            var receiptDetail = receiptDetails.FirstOrDefault(rd => rd.Id == receiptDetailId);
            if (receiptDetail == null || receiptDetail.Status != BorrowStatus.Pending)
                return false;

            // Chuyển trạng thái sang Canceled
            receiptDetail.Status = BorrowStatus.Canceled;
            receiptDetail.Notes = notes;
            await _borrowRepository.UpdateBorrowStatusAsync(receiptDetailId, BorrowStatus.Canceled);

            return true;
        }

        //Lịch sử mượn sách của một user
        public async Task<IEnumerable<object>> GetBorrowHistoryAsync(string userId)
        {
            // Lấy danh sách tất cả sách đã mượn của user từ BorrowReceiptDetail
            var borrowedBooks = await _borrowRepository.GetBorrowedBooksByUserAsync(userId);

            // Kiểm tra nếu không có sách mượn nào
            if (borrowedBooks == null || !borrowedBooks.Any())
            {
                return new List<BorrowReceiptDetail>();
            }
            return borrowedBooks;
        }

        // Thành viên trả sách
        public async Task<bool> ReturnBookAsync(int receiptDetailId)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptDetailId);
            var receiptDetail = receiptDetails.FirstOrDefault(rd => rd.Id == receiptDetailId);
            if (receiptDetail == null || receiptDetail.Status != BorrowStatus.Approved)
                return false;
            receiptDetail.ReturnedDate = DateTime.Now;
            receiptDetail.Notes = "Book has been returned!";
            await _borrowRepository.UpdateBorrowStatusAsync(receiptDetailId, BorrowStatus.Returned);
            return true;
        }

        // Thành viên trả sách và áp dụng phạt
        public async Task<bool> ReturnBookAndApplyPenaltyAsync(int receiptDetailId)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptId2Async(receiptDetailId);

            var receiptDetail = receiptDetails?.FirstOrDefault(rd => rd.Id == receiptDetailId);

            if (receiptDetail == null || receiptDetail.Status != BorrowStatus.Overdue)
                return false;

            receiptDetail.ReturnedDate = DateTime.Now;

            // Add dữ liệu vào bảng penalty
            var daysLate = (receiptDetail.ReturnedDate.Value - receiptDetail.DueDate.Value).Days;

            

            var penalty = new Penalty
            {
                UserId = receiptDetail.BorrowReceipt.UserId,
                BorrowReceiptDetailId = receiptDetail.Id,
                Amount = CalculatePenaltyAmount(receiptDetail),
                Reason = $"Overdue return - {daysLate} days late",
                IssuedDate = DateTime.Now,
                Status = PenaltyStatus.Unpaid
            };

            receiptDetail.Notes = $"Overdue return - {daysLate} days late";

            await _penaltyRepository.AddPenaltyAsync(penalty);

            await _borrowRepository.UpdateBorrowStatusAsync(receiptDetailId, BorrowStatus.Returned);

            return true;
        }

        private decimal CalculatePenaltyAmount(BorrowReceiptDetail detail)
        {
            var overdueDays = (detail.ReturnedDate - detail.DueDate)?.Days ?? 0;
            decimal ratePerDay = 10000m; //tiền phạt
            return overdueDays * ratePerDay;
        }
        // Lấy danh sách tất cả sách yêu cầu mượn, đang mượn, đã trả, quá hạn
        // của user từ BorrowReceiptDetail
        public async Task<IEnumerable<object>> GetAllBorrowBookHistoryAsync(string userId)
        {
            var borrowedBooks = await _borrowRepository.GetLoanHistoryAsync(userId);

            if (borrowedBooks == null || !borrowedBooks.Any())
            {
                return new List<BorrowReceiptDetail>();
            }
            return borrowedBooks;
        }

        // Thành viên gửi yêu cầu gia hạn thời gian trả sách
        public async Task<string> RequestExtendDueDateAsync(int receiptDetailId, string notes)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptDetailId);
            var receiptDetail = receiptDetails.FirstOrDefault(rd => rd.Id == receiptDetailId);
            if (receiptDetail == null || receiptDetail.Status != BorrowStatus.Approved)
                return "Phiếu mượn không hợp lệ hoặc không ở trạng thái đã duyệt.";

            receiptDetail.Status = BorrowStatus.Pending; // Chuyển trạng thái sang Pending để chờ duyệt
            receiptDetail.Notes = notes;
            await _borrowRepository.UpdateBorrowStatusAsync(receiptDetailId, BorrowStatus.Pending);

            return "Yêu cầu gia hạn thời gian trả sách đã được gửi.";
        }

        // Thủ thư xử lý yêu cầu gia hạn thời gian trả sách
        public async Task<bool> ProcessExtendDueDateRequestAsync(int receiptDetailId, bool isApproved, string notes)
        {
            var receiptDetails = await _borrowRepository.GetReceiptDetailsByReceiptIdAsync(receiptDetailId);
            var receiptDetail = receiptDetails.FirstOrDefault(rd => rd.Id == receiptDetailId);
            if (receiptDetail == null || receiptDetail.Status != BorrowStatus.Pending)
                return false;

            if (isApproved)
            {
                if (receiptDetail.DueDate.HasValue)
                {
                    receiptDetail.DueDate = receiptDetail.DueDate.Value.AddDays(30);
                }
                receiptDetail.Status = BorrowStatus.Approved;
            }
            else
            {
                receiptDetail.Status = BorrowStatus.Approved;
            }

            receiptDetail.Notes = notes;
            await _borrowRepository.UpdateBorrowStatusAsync2(receiptDetailId, receiptDetail.Status);

            return true;
        }

        //Lấy danh sách sách quá hạn
        public async Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksAsync()
        {
            return await _borrowRepository.GetOverdueBooksAsync();
        }

        //Lấy danh sách sách quá hạn theo user id
        public async Task<IEnumerable<BorrowReceiptDetail>> GetOverdueBooksByUserAsync(string userId)
        {
            return await _borrowRepository.GetOverdueBooksByUserAsync(userId);
        }
    }
}
