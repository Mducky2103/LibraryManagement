using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.Repositories.Interface;
using LibraryManagement.Services;
using Moq;
using NUnit.Framework;

namespace LibraryManagement.Test
{
    [TestFixture]
    public class BorrowServiceTests
    {
        private Mock<IBorrowRepository> _mockBorrowRepo;
        private Mock<IPenaltyRepository> _mockPenaltyRepo;
        private BorrowService _service;

        [SetUp]
        public void Setup()
        {
            _mockBorrowRepo = new Mock<IBorrowRepository>();
            _mockPenaltyRepo = new Mock<IPenaltyRepository>();
            _service = new BorrowService(_mockBorrowRepo.Object, _mockPenaltyRepo.Object);
        }

        [Test]
        public async Task GetAllReceiptsAsync_ReturnsList()
        {
            _mockBorrowRepo.Setup(x => x.GetAllReceiptsAsync()).ReturnsAsync(new List<object> { new { Id = 1 } });
            var result = await _service.GetAllReceiptsAsync();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task RequestBorrowAsync_ReturnsError_WhenBookListIsEmpty()
        {
            var result = await _service.RequestBorrowAsync("user123", new List<int>());
            Assert.That(result, Is.EqualTo("Danh sách sách mượn không hợp lệ."));
        }

        [Test]
        public async Task RequestBorrowAsync_ReturnsError_IfBookUnavailable()
        {
            _mockBorrowRepo.Setup(x => x.IsBookAvailableAsync(It.IsAny<int>(), 1)).ReturnsAsync(false);
            var result = await _service.RequestBorrowAsync("user123", new List<int> { 101 });
            Assert.That(result, Is.EqualTo("Sách ID 101 hiện không có sẵn."));
        }

        [Test]
        public async Task ApproveBorrowRequestAsync_ReturnsFalse_IfDetailNotFound()
        {
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(It.IsAny<int>())).ReturnsAsync(new List<BorrowReceiptDetail>());
            var result = await _service.ApproveBorrowRequestAsync(5);
            Assert.That(result, Is.False); 
        }

        [Test]
        public async Task CancelBorrowRequestAsync_ReturnsFalse_IfStatusInvalid()
        {
            var details = new List<BorrowReceiptDetail> {
                new BorrowReceiptDetail { Id = 1, Status = BorrowStatus.Approved }
            };
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(It.IsAny<int>())).ReturnsAsync(details);
            var result = await _service.CancelBorrowRequestAsync(1, "Không hợp lệ");
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ReturnBookAsync_ReturnsFalse_IfNotApproved()
        {
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(1))
                .ReturnsAsync(new List<BorrowReceiptDetail> {
                    new BorrowReceiptDetail { Id = 1, Status = BorrowStatus.Pending }
                });
            var result = await _service.ReturnBookAsync(1);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ReturnBookAndApplyPenaltyAsync_AddsPenalty_WhenOverdue()
        {
            var detail = new BorrowReceiptDetail
            {
                Id = 1,
                Status = BorrowStatus.Overdue,
                DueDate = DateTime.Now.AddDays(-3),
                BorrowReceipt = new BorrowReceipt { UserId = "u123" }
            };

            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptId2Async(1)).ReturnsAsync(new List<BorrowReceiptDetail> { detail });
            _mockPenaltyRepo.Setup(x => x.AddPenaltyAsync(It.IsAny<Penalty>())).Returns(Task.CompletedTask);

            var result = await _service.ReturnBookAndApplyPenaltyAsync(1);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RequestExtendDueDateAsync_ReturnsError_IfNotApproved()
        {
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(10)).ReturnsAsync(new List<BorrowReceiptDetail> {
                new BorrowReceiptDetail { Id = 10, Status = BorrowStatus.Pending }
            });

            var result = await _service.RequestExtendDueDateAsync(10, "Xin gia hạn");
            Assert.That(result, Is.EqualTo("Phiếu mượn không hợp lệ hoặc không ở trạng thái đã duyệt."));
        }

        [Test]
        public async Task ProcessExtendDueDateRequestAsync_ApprovesExtension()
        {
            var detail = new BorrowReceiptDetail
            {
                Id = 20,
                Status = BorrowStatus.Pending,
                DueDate = DateTime.Today
            };
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(20))
                .ReturnsAsync(new List<BorrowReceiptDetail> { detail });

            var result = await _service.ProcessExtendDueDateRequestAsync(20, true, "OK");

            Assert.That(result, Is.True);
            Assert.That(detail.DueDate.Value, Is.EqualTo(DateTime.Today.AddDays(30)));
        }

        [Test]
        public async Task ProcessExtendDueDateRequestAsync_RejectsExtension()
        {
            var detail = new BorrowReceiptDetail
            {
                Id = 30,
                Status = BorrowStatus.Pending,
                DueDate = DateTime.Today.AddDays(10)
            };
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(30))
                .ReturnsAsync(new List<BorrowReceiptDetail> { detail });

            var result = await _service.ProcessExtendDueDateRequestAsync(30, false, "Từ chối");

            Assert.That(result, Is.True);
            Assert.That(detail.Status, Is.EqualTo(BorrowStatus.Approved));
        }

        [Test]
        public async Task GetBorrowHistoryAsync_ReturnsList()
        {
            var fakeHistory = new List<object> { new { BookId = 1 }, new { BookId = 2 } };
            _mockBorrowRepo.Setup(x => x.GetBorrowedBooksByUserAsync("userABC")).ReturnsAsync(fakeHistory);

            var result = await _service.GetBorrowHistoryAsync("userABC");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task RequestBorrowAsync_ReturnsSuccess()
        {
            _mockBorrowRepo.Setup(x => x.IsBookAvailableAsync(It.IsAny<int>(), 1)).ReturnsAsync(true);
            _mockBorrowRepo.Setup(x => x.AddBorrowRequestAsync(It.IsAny<BorrowReceipt>()))
                .Returns(Task.CompletedTask);

            var result = await _service.RequestBorrowAsync("user1", new List<int> { 1, 2 });

            Assert.That(result, Is.EqualTo("Yêu cầu mượn sách đã được gửi."));
        }

        [Test]
        public async Task ReturnBookAsync_ReturnsTrue_WhenApproved()
        {
            var detail = new BorrowReceiptDetail
            {
                Id = 99,
                Status = BorrowStatus.Approved
            };
            _mockBorrowRepo.Setup(x => x.GetReceiptDetailsByReceiptIdAsync(99))
                .ReturnsAsync(new List<BorrowReceiptDetail> { detail });

            var result = await _service.ReturnBookAsync(99);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task GetOverdueBooksByUserAsync_ReturnsList()
        {
            _mockBorrowRepo.Setup(x => x.GetOverdueBooksByUserAsync("user999"))
                .ReturnsAsync(new List<object> { new { BookId = 10 } });

            var result = await _service.GetOverdueBooksByUserAsync("user999");

            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task RequestBorrowAsync_ReturnsError_IfOneOfMultipleBooksUnavailable()
        {
            _mockBorrowRepo.Setup(x => x.IsBookAvailableAsync(101, 1)).ReturnsAsync(true);
            _mockBorrowRepo.Setup(x => x.IsBookAvailableAsync(102, 1)).ReturnsAsync(false); 

            var result = await _service.RequestBorrowAsync("user456", new List<int> { 101, 102 });

            Assert.That(result, Is.EqualTo("Sách ID 102 hiện không có sẵn."));
        }


    }
}
