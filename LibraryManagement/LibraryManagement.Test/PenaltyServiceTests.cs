using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Repositories.Interface;
using LibraryManagement.ViewModels;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class PenaltyServiceTests
    {
        private Mock<IPenaltyRepository> _mockPenaltyRepo;
        private PenaltyService _service;

        [SetUp]
        public void Setup()
        {
            _mockPenaltyRepo = new Mock<IPenaltyRepository>();
            _service = new PenaltyService(_mockPenaltyRepo.Object);
        }

        [Test]
        public async Task GetAllPenaltiesAsync_ReturnsPenaltyList()
        {
            // Arrange
            var mockPenalties = new List<PenaltyVm>
            {
                new PenaltyVm { Id = 1, Username = "user1", Amount = 50000 },
                new PenaltyVm { Id = 2, Username = "user2", Amount = 100000 }
            };
            _mockPenaltyRepo.Setup(repo => repo.GetAllPenaltiesAsync()).ReturnsAsync(mockPenalties);

            // Act
            var result = await _service.GetAllPenaltiesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(2).Items);
        }

        [Test]
        public async Task GetPenaltiesByUserIdAsync_ReturnsPenaltiesForUser()
        {
            // Arrange
            var userId = "user123";
            var userPenalties = new List<PenaltyVm>
            {
                new PenaltyVm { Id = 3, Username = "user123", Amount = 25000 }
            };
            _mockPenaltyRepo.Setup(repo => repo.GetPenaltiesByUserIdAsync(userId)).ReturnsAsync(userPenalties);

            // Act
            var result = await _service.GetPenaltiesByUserIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Exactly(1).Items);
            Assert.That(result.First().Username, Is.EqualTo("user123"));
        }

        [Test]
        public async Task GetPenaltiesByUserIdAsync_ReturnsEmptyList_WhenNoPenalty()
        {
            // Arrange
            var userId = "no_penalty_user";
            _mockPenaltyRepo.Setup(r => r.GetPenaltiesByUserIdAsync(userId)).ReturnsAsync(new List<PenaltyVm>());

            // Act
            var result = await _service.GetPenaltiesByUserIdAsync(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(0));
        }
    }
}
