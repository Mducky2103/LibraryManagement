using LibraryManagement.Services;
using LibraryManagement.ViewModels;
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

        [HttpGet("get-all-authors")]
        public async Task<IActionResult> GetAllAuthor()
        {
            var authors = await _authorService.GetAllAuthorsAsync();

            return Ok(authors);
        }

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

        [HttpPost("add-author")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorVm authorVm)
        {
            await _authorService.AddAuthorAsync(authorVm);

            return Ok();
        }

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

        [HttpDelete("delete-author/{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthorAsync(id);

            return Ok();
        }
    }
}
