using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [Authorize]
        [HttpGet("get-all-authors")]
        public async Task<IActionResult> GetAllAuthor()
        {
            var authors = await _authorService.GetAllAuthorsAsync();

            return Ok(authors);
        }

        [Authorize]
        [HttpGet("get-author-by-id/{id}")]
        public async Task<IActionResult> GetAuhtorById(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpPost("add-author")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorVm authorVm)
        {
            await _authorService.AddAuthorAsync(authorVm);

            return Ok();
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpPut("update-author/{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorVm authorVm)
        {
            try
            {
                await _authorService.UpdateAuthorAsync(id, authorVm);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpDelete("delete-author/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthorAsync(id);

            return Ok();
        }
    }
}
