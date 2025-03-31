namespace LibraryManagement.ViewModels
{
    public class BorrowRequestModel
    {
        public string UserId { get; set; }
        public List<int> BookIds { get; set; }
    }
}
