using BookRentalService.Model;

namespace BookRentalService.DTO
{
    public class StatResult
    {
        public Book MostPopularBook { get; set; }
        public Book LeastPopularBook { get; set; }
        public Book MostOverdueBook { get; set; }
    }
}