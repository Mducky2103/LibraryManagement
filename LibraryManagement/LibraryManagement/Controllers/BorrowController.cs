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

        // Xem danh sách phiếu mượn 
        [HttpGet("borrowed-books/{userId}")]
        public async Task<IActionResult> GetBorrowedBooks(string userId)
        {
            var borrowedBooks = await _borrowService.GetBorrowHistoryAsync(userId);
            if (!borrowedBooks.Any())
            {
                return NotFound("No borrowed books found.");
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
                return NotFound("No borrow history found.");
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

        // Thành viên trả sách
        [HttpPut("return-book/{detailId}")]
        public async Task<IActionResult> ReturnBook(int detailId)
        {
            bool isReturned = await _borrowService.ReturnBookAsync(detailId);
            if (!isReturned)
                return NotFound("Không tìm thấy thông tin sách đã mượn.");

            return Ok(new { message = "Sách đã được trả thành công." });
        }

        //Gia hạn thời gian trả sách
        [HttpPost("request-extend-due-date/{detailId}")]
        public async Task<IActionResult> RequestExtendDueDate(int detailId, string notes)
        {
            var result = await _borrowService.RequestExtendDueDateAsync(detailId, notes);
            return Ok(new { message = result });
        }
    }
}
