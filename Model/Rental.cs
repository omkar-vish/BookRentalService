namespace BookRentalService.Model
{
    public class Rental
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime RentedOnDate { get; set; }
        public int NoOfDays { get; set; } 
        public DateTime? ReturnDate { get; set; }
        public bool IsOverdue { get; set; } // => ReturnDate == null && RentedOnDate.AddDays(NoOfDays) < DateTime.Now;
        public Book Book { get; set; } // Navigation property for EF Core
        public User User { get; set; } // Navigation property for EF Core
    }
}
