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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromForm] BookVm bookVm)
        {
            if (bookVm.Picture.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", bookVm.Picture.FileName);

                using (var stream = System.IO.File.Create(path))
                {
                    await bookVm.Picture.CopyToAsync(stream);
                }

                bookVm.Image = bookVm.Picture.FileName;
            }
            else
            {
                bookVm.Image = "";
            }

            await _bookService.AddBookAsync(bookVm);

            return Ok();
        }

        [HttpPut("update-book/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookVm bookVm)
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

            // Trả về ảnh dưới dạng ContentResult với MIME type tương ứng
            return File(imageBytes, "image/jpeg");
        }
    }
}
