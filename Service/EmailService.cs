using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Repository;

namespace BookRentalService.Service
{
    public class EmailService : IEmailService
    {
        private readonly IRentalRepository _rentalRepository;

        public EmailService(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task SendLastMonthOverdueNotificationAsync()
        {
            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);

            var overdueRentals = await _rentalRepository.GetOverdueRentalsAsync(currentMonthStart, lastMonthStart);

            // Send email service. From above we get user email id and book details.

        }
    }
}
