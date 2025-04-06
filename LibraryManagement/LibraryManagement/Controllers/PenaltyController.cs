using LibraryManagement.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PenaltyController : ControllerBase
    {
        private readonly IPenaltyService _penaltyService;

        public PenaltyController(IPenaltyService penaltyService)
        {
            _penaltyService = penaltyService;
        }

        //Lấy danh sách tất cả các khoản phạt
        [HttpGet("all-penalties")]
        [Authorize(Roles = "Admin, Librarian")]
        public async Task<IActionResult> GetAllPenalties()
        {
            var penalties = await _penaltyService.GetAllPenaltiesAsync();
            if (penalties == null || !penalties.Any())
            {
                return NotFound("Không tìm thấy thông tin");
            }
            return Ok(penalties);
        }

        //Lấy danh sách các khoản phạt của một người dùng theo ID
        [HttpGet("user-penalties/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetPenaltiesByUserId(string userId)
        {
            var penalties = await _penaltyService.GetPenaltiesByUserIdAsync(userId);
            if (penalties == null || !penalties.Any())
            {
                return NotFound("Không tìm thấy thông tin");
            }
            return Ok(penalties);
        }
    }
}
