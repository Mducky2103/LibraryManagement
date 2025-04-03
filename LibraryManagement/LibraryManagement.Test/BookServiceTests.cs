using LibraryManagement.Data;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class BookServiceTests
    {
        private LibraryDbContext _context;
        private BookService _bookService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new LibraryDbContext(options);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            SeedData();

            var repository = new BookRepository(_context);
            _bookService = new BookService(repository);
        }

        private void SeedData()
        {
            var category = new Category { Id = 1, Name = "Fiction" };
            var author = new Author { Id = 1, Name = "Author 1", Address = "USA" };
            var books = new[]
            {
                new Book { Id = 1, Name = "Book 1", Description = "This is book 1", Image = "image1.jpg", CategoryId = 1, AuthorId = 1, Price = 100000, YearPublished = 2020, Quantity = 10, IsAvailable = true },
                new Book { Id = 2, Name = "Book 2", Description = "This is book 1", Image = "image2.jpg", CategoryId = 1, AuthorId = 1, Price = 150000, YearPublished = 2021, Quantity = 5, IsAvailable = true }
            };

            _context.Categories.Add(category);
            _context.Authors.Add(author);
            _context.Books.AddRange(books);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task GetAllBooksAsync_ReturnsListOfBookVms()
        {
            // Act
            var result = await _bookService.GetAllBooksAsync();

            // Assert
            var bookVms = result.ToList();
            Assert.That(bookVms.Count, Is.EqualTo(2), "Should return 2 books");
            Assert.That(bookVms[0].Name, Is.EqualTo("Book 1"), "First book name should match");
            Assert.That(bookVms[0].CategoryName, Is.EqualTo("Fiction"), "First book category should match");
            Assert.That(bookVms[0].AuthorName, Is.EqualTo("Author 1"), "First book author should match");
        }

        [Test]
        public async Task GetBookByIdAsync_ReturnsBookVm_WhenBookExists()
        {
            // Act
            var result = await _bookService.GetBookByIdAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null, "Book should exist");
            Assert.That(result.Id, Is.EqualTo(1), "Book ID should match");
            Assert.That(result.Name, Is.EqualTo("Book 1"), "Book name should match");
        }

        [Test]
        public async Task AddBookAsync_AddsBookToDatabase()
        {
            // Arrange
            var bookVm = new BookAddVm
            {
                Name = "New Book",
                Description = "New Description",
                YearPublished = 2023,
                Price = 200000,
                Quantity = 3,
                Image = "newimage.jpg",
                IsAvailable = true,
                CategoryId = 1,
                AuthorId = 1
            };

            // Act
            await _bookService.AddBookAsync(bookVm);
            var books = await _context.Books.ToListAsync();

            // Assert
            Assert.That(books.Count, Is.EqualTo(3), "Should have 3 books after adding");
            var addedBook = books.FirstOrDefault(b => b.Name == "New Book");
            Assert.That(addedBook, Is.Not.Null, "New book should exist");
            Assert.That(addedBook.Price, Is.EqualTo(200000), "Price should match");
        }

        [Test]
        public async Task UpdateBookAsync_UpdatesBook_WhenBookExists()
        {
            // Arrange
            var bookVm = new BookAddVm
            {
                Name = "Updated Book",
                Description = "Updated Description",
                YearPublished = 2022,
                Price = 250000,
                Quantity = 7,
                Image = "updatedimage.jpg",
                IsAvailable = false,
                CategoryId = 1,
                AuthorId = 1
            };

            // Act
            await _bookService.UpdateBookAsync(1, bookVm);
            var updatedBook = await _context.Books.FindAsync(1);

            // Assert
            Assert.That(updatedBook.Name, Is.EqualTo("Updated Book"), "Name should be updated");
            Assert.That(updatedBook.Price, Is.EqualTo(250000), "Price should be updated");
            Assert.That(updatedBook.IsAvailable, Is.False, "Availability should be updated");
        }

        [Test]
        public void UpdateBookAsync_ThrowsException_WhenBookDoesNotExist()
        {
            // Arrange
            var bookVm = new BookAddVm { Name = "Updated Book" };

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () => await _bookService.UpdateBookAsync(999, bookVm));
            Assert.That(exception.Message, Is.EqualTo("Not found book."), "Exception message should match");
        }

        [Test]
        public async Task DeleteBookAsync_RemovesBookFromDatabase()
        {
            // Act
            await _bookService.DeleteBookAsync(1);
            var books = await _context.Books.ToListAsync();

            // Assert
            Assert.That(books.Count, Is.EqualTo(1), "Should have 1 book left after deletion");
            Assert.That(books.Any(b => b.Id == 1), Is.False, "Book with ID 1 should be deleted");
        }
    }
}
