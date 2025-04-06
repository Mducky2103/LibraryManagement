namespace LibraryManagement.ViewModels
{
    public class PenaltyVm
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int BorrowReceiptDetailId { get; set; }
        public string BookName { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime IssuedDate { get; set; }
        public string Status { get; set; }
    }
}
