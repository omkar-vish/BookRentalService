namespace BookRentalService.DTO
{
    public class RentalHistoryDto
    {
        public int RentalId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public DateTime RentedOn { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public bool IsOverdue { get; set; }
    }
}