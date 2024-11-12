namespace BookRentalService.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
        public int TimesRented { get; set; }
        public DateTime RowVersion { get; set; }
    }
}