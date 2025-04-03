using LibraryManagement.Services.Interface;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }
        //Xem tất cả phiếu mượn
        [HttpGet("view-all-receipts")]
        public async Task<IActionResult> GetAllReceipts()
        {
            var receipts = await _borrowService.GetAllReceiptsAsync();
            if (!receipts.Any())
            {
                return NotFound("Không tìm thấy thông tin");
            }
            return Ok(receipts);
        }

        //xem thông tin một phiếu mượn theo ID
        [HttpGet("view-all-receipt-by-id/{id}")]
        public async Task<IActionResult> ViewReceiptById(int id)
        {
            var receipt = await _borrowService.GetReceiptByIdAsync(id);
            if (receipt == null)
            {
                return NotFound("Không tìm thấy thông tin");
            }
            return Ok(receipt);
        }

        //Xem danh sách sách quá hạn
        [HttpGet("overdue-books")]
        public async Task<IActionResult> GetOverdueBooks()
        {
            var overdueBooks = await _borrowService.GetOverdueBooksAsync();
            if (!overdueBooks.Any())
            {
                return NotFound("Không tìm thấy sách quá hạn");
            }
            return Ok(overdueBooks);
        }

        //Xem danh sách sách quá hạn của một thành viên
        [HttpGet("overdue-books/{userId}")]
        public async Task<IActionResult> GetOverdueBooksByUser(string userId)
        {
            var overdueBooks = await _borrowService.GetOverdueBooksByUserAsync(userId);
            if (!overdueBooks.Any())
            {
                return NotFound("Không tìm thấy sách quá hạn");
            }
            return Ok(overdueBooks);
        }

        // Xem danh sách phiếu mượn 
        [HttpGet("borrowed-books/{userId}")]
        public async Task<IActionResult> GetBorrowedBooks(string userId)
        {
            var borrowedBooks = await _borrowService.GetBorrowHistoryAsync(userId);
            if (!borrowedBooks.Any())
            {
                return NotFound("Không tìm thấy danh sách phiếu mượn");
            }
            return Ok(borrowedBooks);
        }

        //Xem lịch sử mượn sách (tất cả sách đã mượn, đã trả, đang chờ yêu cầu mượn, quá hạn)
        [HttpGet("borrow-history/{userId}")]
        public async Task<IActionResult> GetBorrowHistory(string userId)
        {
            var borrowHistory = await _borrowService.GetAllBorrowBookHistoryAsync(userId);
            if (!borrowHistory.Any())
            {
                return NotFound("Không tìm thấy lịch sử mượn sách.");
            }
            return Ok(borrowHistory);
        }

        // Lấy thông tin chi tiết của một phiếu mượn theo ID
        [HttpGet("Get-detail-borrow-by-id{id}")]
        public async Task<IActionResult> GetReceiptById(int id)
        {
            var receipt = await _borrowService.GetReceiptDetailsByReceiptIdAsync(id);
            if (receipt == null)
                return NotFound("Phiếu mượn không tồn tại.");

            return Ok(receipt);
        }

        // Thành viên gửi yêu cầu mượn sách
        [HttpPost("request-borrow-book")]
        public async Task<IActionResult> RequestBorrow([FromBody] BorrowRequestModel request)
        {
            if (request == null || !request.BookIds.Any())
                return BadRequest("Danh sách sách không hợp lệ.");

            var result = await _borrowService.RequestBorrowAsync(request.UserId, request.BookIds);
            return Ok(new { message = result });
        }

        // Thủ thư phê duyệt yêu cầu mượn sách
        [HttpPut("approve-borrow-book/{detailId}")]
        public async Task<IActionResult> ApproveBorrowRequest(int detailId)
        {
            bool isApproved = await _borrowService.ApproveBorrowRequestAsync(detailId);
            if (!isApproved)
                return NotFound("Không tìm thấy yêu cầu mượn sách.");

            return Ok(new { message = "Yêu cầu mượn sách đã được phê duyệt." });
        }

        // Thủ thư từ chối yêu cầu mượn sách (Chuyển trạng thái thành Canceled)
        [HttpPut("cancel/{detailId}")]
        public async Task<IActionResult> CancelBorrowRequest(int detailId, string notes)
        {
            bool isCanceled = await _borrowService.CancelBorrowRequestAsync(detailId, notes);
            if (!isCanceled)
                return NotFound("Không tìm thấy yêu cầu hoặc yêu cầu không thể hủy.");

            return Ok(new { message = "Yêu cầu mượn sách đã bị từ chối." });
        }

        // Thành viên trả sách
        [HttpPut("return-book/{detailId}")]
        public async Task<IActionResult> ReturnBook(int detailId)
        {
            bool isReturned = await _borrowService.ReturnBookAsync(detailId);
            if (!isReturned)
                return NotFound("Không tìm thấy thông tin sách đã mượn.");

            return Ok(new { message = "Sách đã được trả thành công." });
        }

        // Thành viên trả sách và áp dụng phạt
        [HttpPut("return-book-and-apply-penalty/{detailId}")]
        public async Task<IActionResult> ReturnBookAndApplyPenalty(int detailId)
        {
            bool isReturned = await _borrowService.ReturnBookAndApplyPenaltyAsync(detailId);
            if (!isReturned)
                return NotFound("Không tìm thấy thông tin sách đã mượn.");
            return Ok(new { message = "Sách đã được trả thành công và áp dụng phạt." });
        }

        //Gia hạn thời gian trả sách
        [HttpPost("request-extend-due-date/{detailId}")]
        public async Task<IActionResult> RequestExtendDueDate(int detailId, string notes)
        {
            var result = await _borrowService.RequestExtendDueDateAsync(detailId, notes);
            return Ok(new { message = result });
        }

        // Thủ thư phê duyệt yêu cầu gia hạn thời gian trả sách
        [HttpPut("approve-extend-due-date/{detailId}")]
        public async Task<IActionResult> ApproveExtendDueDateRequest(int detailId, bool isApproved, string notes)
        {
            bool result = await _borrowService.ProcessExtendDueDateRequestAsync(detailId, isApproved, notes);
            if (!result)
                return NotFound("Không tìm thấy yêu cầu gia hạn thời gian trả sách.");
            return Ok(new { message = "Yêu cầu gia hạn thời gian trả sách đã được phê duyệt." });
        }
    }
}
