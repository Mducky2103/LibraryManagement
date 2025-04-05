using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [Authorize]
        [HttpGet("get-all-books")]
        public async Task<IActionResult> GetAllBook()
        {
            var books = await _bookService.GetAllBooksAsync();

            foreach (var book in books)
            {
                if (!string.IsNullOrEmpty(book.Image))
                {
                    book.Image =  book.Image;
                }
            }

            return Ok(books);
        }

        [Authorize]
        [HttpGet("get-book-by-id/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(book.Image))
            {
                book.Image = book.Image;
            }

            return Ok(book);
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromForm] BookAddVm bookVm, IFormFile picture)
        {
            if (picture.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", picture.FileName);

                using (var stream = System.IO.File.Create(path))
                {
                    await picture.CopyToAsync(stream);
                }

                bookVm.Image = picture.FileName;
            }
            else
            {
                bookVm.Image = "";
            }

            await _bookService.AddBookAsync(bookVm);

            return Ok();
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpPut("update-book/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookAddVm bookVm, IFormFile picture)
        {
            try
            {
                await _bookService.UpdateBookAsync(id, bookVm);

                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Admin, Librarian")]
        [HttpDelete("delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("get-book-image/{imageName}")]
        public IActionResult GetBookImage(string imageName)
        {
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            var imageBytes = System.IO.File.ReadAllBytes(imagePath);

            return File(imageBytes, "image/jpeg");
        }

        [Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<BookVm>>> SearchBooks(string searchTerm)
        {
            var books = await _bookService.SearchBooksAsync(searchTerm);
            return Ok(books);
        }

        [Authorize]
        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<BookVm>>> GetBooksByCategory(int categoryId)
        {
            var books = await _bookService.GetBooksByCategoryAsync(categoryId);
            return Ok(books);
        }

        [Authorize]
        [HttpGet("by-author/{authorId}")]
        public async Task<ActionResult<IEnumerable<BookVm>>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookService.GetBooksByAuthorAsync(authorId);
            return Ok(books);
        }
    }
}
