namespace LibraryManagement.ViewModels
{
    public class BookAddVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int YearPublished { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}
