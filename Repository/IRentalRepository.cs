using BookRentalService.DTO;
using BookRentalService.Model;

namespace BookRentalService.Repository
{
    public interface IRentalRepository
    {
        Task<List<RentalHistoryDto>> GetAllRentalHistoryAsync();
        Task AddAsync(Rental rental);
        Task<List<RentalHistoryDto>> GetBookRentalHistoryAsync(int bookId);
        Task<Rental> GetByIdAsync(int rentalId);
        Task<List<UserOverDueRentalDto>> GetOverdueRentalsAsync(DateTime currentMonthStart, DateTime lastMonthStart);
        Task<List<Rental>> GetPendingOverdueRentalsAsync(TimeSpan overduePeriod, DateTime currentDate);
        Task<List<RentalHistoryDto>> GetRentalHistoryAsync(int userId);
        Task UpdateAsync(Rental rental);
        Task UpdateRangeAsync(List<Rental> overdueRentals);
    }
}