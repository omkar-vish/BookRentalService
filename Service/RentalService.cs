using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Model;
using BookRentalService.Repository;
using Microsoft.Extensions.Options;

namespace BookRentalService.Service
{
    public class RentalService : IRentalService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly AppSettings _appSettings;

        public RentalService(IBookRepository bookRepository, IRentalRepository rentalRepository, IOptions<AppSettings> appSettings)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<List<RentalHistoryDto>> GetAllRentalHistoryAsync()
        {
            return await _rentalRepository.GetAllRentalHistoryAsync();
        }
        public async Task<List<RentalHistoryDto>> GetRentalHistoryAsync(int userId)
        {
            return await _rentalRepository.GetRentalHistoryAsync(userId);
        }

        public async Task<Rental> RentBookAsync(int userId, int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (!book.IsAvailable)
                throw new InvalidOperationException("Book is currently unavailable.");

            var maxDaysRentedDuration = _appSettings.MaxDaysRentDuration;
            var rental = new Rental
            {
                UserId = userId,
                BookId = bookId,
                NoOfDays = maxDaysRentedDuration,
                RentedOnDate = DateTime.Now
            };

            book.IsAvailable = false;
            await _bookRepository.UpdateAsync(book);
            await _rentalRepository.AddAsync(rental);

            return rental;
        }

        public async Task<bool> ReturnBookAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);
            if (rental == null || rental.ReturnDate != null)
                throw new InvalidOperationException("Invalid or already returned rental.");

            rental.ReturnDate = DateTime.Now;
            var book = await _bookRepository.GetByIdAsync(rental.BookId);
            book.IsAvailable = true;

            await _bookRepository.UpdateAsync(book);
            await _rentalRepository.UpdateAsync(rental);

            return true;
        }

        public async Task<List<RentalHistoryDto>> GetBookRentalHistoryAsync(int bookId)
        {
            return await _rentalRepository.GetBookRentalHistoryAsync(bookId);
        }

        public async Task MarkOverdueRentalsAsync()
        {
            var maxDaysRentedDuration = _appSettings.MaxDaysRentDuration;
            var overduePeriod = TimeSpan.FromDays(maxDaysRentedDuration);
            var currentDate = DateTime.UtcNow;

            var overdueRentals = _rentalRepository.GetPendingOverdueRentalsAsync(overduePeriod, currentDate).Result;

            foreach (var rental in overdueRentals)
            {
                rental.IsOverdue = true;
            }

            await _rentalRepository.UpdateRangeAsync(overdueRentals);
        }
    }
}