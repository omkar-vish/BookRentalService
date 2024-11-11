using BookRentalService.DTO;
using BookRentalService.Model;

namespace BookRentalService.Interface
{
    public interface IRentalService
    {
        Task<List<RentalHistoryDto>> GetAllRentalHistoryAsync();
        Task<List<RentalHistoryDto>> GetBookRentalHistoryAsync(int bookId);  
        Task<List<RentalHistoryDto>> GetRentalHistoryAsync(int userId);
        Task MarkOverdueRentalsAsync();
        Task<Rental> RentBookAsync(int userId, int bookId);
        Task<bool> ReturnBookAsync(int rentalId);
    }
}