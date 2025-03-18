namespace LibraryManagement.Models
{
    public class ReturnHistory
    {
        public int Id { get; set; }
        public int BorrowReceiptId { get; set; }
        public BorrowReceipt BorrowReceipt { get; set; }
        public DateTime ReturnedDate { get; set; }
    }
}
