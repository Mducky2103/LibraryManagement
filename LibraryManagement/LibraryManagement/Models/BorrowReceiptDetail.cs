namespace LibraryManagement.Models
{
    public class BorrowReceiptDetail
    {
        public int Id { get; set; }
        public int BorrowReceiptId { get; set; }
        public BorrowReceipt BorrowReceipt { get; set; }
        public int BookId { get; set; }
        public Book Books { get; set; }
        public DateTime BorrowedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public string? Notes { get; set; }
        public ICollection<ReturnedBooks> ReturnedBooks { get; set; }
    }
}
