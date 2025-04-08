using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class AuthorServiceTests
    {
        private Mock<IAuthorRepository> _mockRepo;
        private AuthorService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IAuthorRepository>();
            _service = new AuthorService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllAuthorsAsync_ReturnsAllAuthors()
        {
            var authors = new List<Author>
            {
                new Author { Id = 1, Name = "Author A", Address = "Address A" },
                new Author { Id = 2, Name = "Author B", Address = "Address B" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(authors);

            var result = await _service.GetAllAuthorsAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Author A"));
        }

        [Test]
        public async Task GetAuthorByIdAsync_ReturnsCorrectAuthor()
        {
            var author = new Author { Id = 1, Name = "Author A", Address = "Address A" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(author);

            var result = await _service.GetAuthorByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Author A"));
        }

        [Test]
        public async Task GetAuthorByIdAsync_ReturnsNull_WhenNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Author)null);

            var result = await _service.GetAuthorByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddAuthorAsync_CallsRepositoryWithCorrectData()
        {
            Author savedAuthor = null;

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Author>()))
                     .Callback<Author>(a => savedAuthor = a)
                     .Returns(Task.CompletedTask);

            var authorVm = new AuthorVm { Name = "New Author", Address = "New Address" };
            await _service.AddAuthorAsync(authorVm);

            Assert.That(savedAuthor, Is.Not.Null);
            Assert.That(savedAuthor.Name, Is.EqualTo("New Author"));
            Assert.That(savedAuthor.Address, Is.EqualTo("New Address"));
        }

        [Test]
        public async Task UpdateAuthorAsync_UpdatesExistingAuthor()
        {
            var existingAuthor = new Author { Id = 1, Name = "Old", Address = "Old" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingAuthor);
            _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Author>())).Returns(Task.CompletedTask);

            var updatedVm = new AuthorVm { Name = "Updated", Address = "Updated Addr" };
            await _service.UpdateAuthorAsync(1, updatedVm);

            Assert.That(existingAuthor.Name, Is.EqualTo("Updated"));
            Assert.That(existingAuthor.Address, Is.EqualTo("Updated Addr"));
        }

        [Test]
        public void UpdateAuthorAsync_ThrowsException_WhenAuthorNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Author)null);

            var vm = new AuthorVm { Name = "Test", Address = "Test" };

            Assert.ThrowsAsync<Exception>(async () =>
                await _service.UpdateAuthorAsync(99, vm), "Not found author.");
        }

        [Test]
        public async Task DeleteAuthorAsync_CallsRepository()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await _service.DeleteAuthorAsync(1);

            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
