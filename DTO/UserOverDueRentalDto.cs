using BookRentalService.Model;

namespace BookRentalService.DTO
{
    public class UserOverDueRentalDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int RentalId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime RentedOn { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public bool IsOverdue { get; set; }
    }
}