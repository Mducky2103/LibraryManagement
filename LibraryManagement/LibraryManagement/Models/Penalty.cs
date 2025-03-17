using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public int BorrowReceiptDetailId { get; set; }
        public BorrowReceiptDetail BorrowReceiptDetail { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime IssuedDate { get; set; }
        public PenaltyStatus Status { get; set; } = PenaltyStatus.Unpaid;
    }
    public enum PenaltyStatus
    {
        Unpaid,
        Paid,
        Canceled
    }
}
