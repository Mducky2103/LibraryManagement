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
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepoMock;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_categoryRepoMock.Object);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsAllCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" }
            };

            _categoryRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Fiction"));

        }

        [Test]
        public async Task GetCategoryByIdAsync_ValidId_ReturnsCategory()
        {
            var category = new Category { Id = 1, Name = "History" };
            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await _categoryService.GetCategoryByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("History"));
        }

        [Test]
        public async Task GetCategoryByIdAsync_InvalidId_ReturnsNull()
        {
            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            var result = await _categoryService.GetCategoryByIdAsync(999);

            Assert.That(result, Is.Null); // dùng khi test category không tồn tại
        }

        [Test]
        public async Task AddCategoryAsync_ValidCategory_CallsAddAsync()
        {
            var categoryVm = new CategoryVm { Name = "Adventure" };

            await _categoryService.AddCategoryAsync(categoryVm);

            _categoryRepoMock.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == "Adventure")), Times.Once);
        }

        [Test]
        public async Task UpdateCategoryAsync_ExistingCategory_UpdatesSuccessfully()
        {
            var existing = new Category { Id = 1, Name = "Old" };
            var updateVm = new CategoryVm { Name = "Updated" };

            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existing);

            await _categoryService.UpdateCategoryAsync(1, updateVm);

            _categoryRepoMock.Verify(repo => repo.UpdateAsync(1, It.Is<Category>(c => c.Name == "Updated")), Times.Once);
        }

        [Test]
        public void UpdateCategoryAsync_CategoryNotFound_ThrowsException()
        {
            _categoryRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Category)null);

            Assert.ThrowsAsync<Exception>(() => _categoryService.UpdateCategoryAsync(1, new CategoryVm()));
        }

        [Test]
        public async Task DeleteCategoryAsync_CallsDelete()
        {
            await _categoryService.DeleteCategoryAsync(1);

            _categoryRepoMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Test]
        public async Task GetCategoryWithBooksAsync_ReturnsCorrectData()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Fantasy",
                Books = new List<Book>
                {
                    new Book
                    {
                        Id = 101,
                        Name = "The Hobbit",
                        Author = new Author { Id = 1, Name = "Tolkien" },
                        Description = "Fantasy book",
                        Price = 20,
                        Quantity = 5,
                        CategoryId = 1,
                        AuthorId = 1
                    }
                }
            };

            _categoryRepoMock.Setup(repo => repo.GetWithBooksAsync(1)).ReturnsAsync(category);

            var result = await _categoryService.GetCategoryWithBooksAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Fantasy"));
            Assert.That(result.Books.Count, Is.EqualTo(1));
            Assert.That(result.Books.First().Name, Is.EqualTo("The Hobbit"));
        }

        [Test]
        public async Task GetCategoryWithBooksAsync_NotFound_ReturnsNull()
        {
            _categoryRepoMock.Setup(repo => repo.GetWithBooksAsync(999)).ReturnsAsync((Category)null);

            var result = await _categoryService.GetCategoryWithBooksAsync(999);

            Assert.That(result, Is.Null);
        }
    }
}
