namespace LibraryManagement.Models
{
    public class ReturnedBooks
    {
        public int Id { get; set; }
        public int BorrowReceiptId { get; set; }
        public BorrowReceipt BorrowReceipts { get; set; }
        public int BorrowReceiptDetailId { get; set; }
        public BorrowReceiptDetail BorrowReceiptDetails { get; set; }
        public DateTime ReturnedDate { get; set; }
        public bool IsReturned { get; set; }
    }
}
