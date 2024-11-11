using BookRentalService.DTO;
using BookRentalService.Model;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BookRentalService.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly BookRentalDbContext _context;

        public RentalRepository(BookRentalDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<Rental> GetByIdAsync(int rentalId)
        {
            return await _context.Rentals
                        .Where(rental => rental.Id.Equals(rentalId))
                        .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Rental rental)
        {
            _context.Update(rental);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<Rental> overdueRentals)
        {
            _context.UpdateRange(overdueRentals);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RentalHistoryDto>> GetRentalHistoryAsync(int userId)
        {
            var rentalHistory = await _context.Rentals
                .Where(r => r.UserId == userId)
                .Include(r => r.Book) // Include related Book data
                .Select(r => new RentalHistoryDto
                {
                    RentalId = r.Id,
                    BookId = r.BookId,
                    UserId = r.UserId,
                    BookTitle = r.Book.Title,
                    RentedOn = r.RentedOnDate,
                    ReturnedOn = r.ReturnDate,
                    IsOverdue = r.IsOverdue
                })
                .OrderByDescending(r => r.RentedOn) // Order by most recent rentals
                .ToListAsync();

            return rentalHistory;
        }

        public async Task<List<RentalHistoryDto>> GetBookRentalHistoryAsync(int bookId)
        {
            var bookRentalHistory = await _context.Rentals
                       .Where(r => r.BookId == bookId)
                       .Include(r => r.Book)  // Include related Book data
                       .Select(r => new RentalHistoryDto
                       {
                           RentalId = r.Id,
                           BookId = r.BookId,
                           UserId = r.UserId,
                           BookTitle = r.Book.Title,
                           RentedOn = r.RentedOnDate,
                           ReturnedOn = r.ReturnDate,
                           IsOverdue = r.IsOverdue
                       })
                       .OrderByDescending(r => r.RentedOn) // Order by most recent rentals
                       .ToListAsync();

            return bookRentalHistory;
        }

        public async Task<List<Rental>> GetPendingOverdueRentalsAsync(TimeSpan overduePeriod, DateTime currentDate)
        {
            return await _context.Rentals
                       .Where(r => !r.ReturnDate.HasValue &&
                                   !r.IsOverdue &&
                                   r.RentedOnDate.Add(overduePeriod) < currentDate)
                       .ToListAsync();
        }

        public async Task<List<UserOverDueRentalDto>> GetOverdueRentalsAsync(DateTime currentMonthStart, DateTime lastMonthStart)
        {

            return await _context.Rentals
                        .Where(r => r.RentedOnDate >= currentMonthStart && r.RentedOnDate <= currentMonthStart.AddMonths(1).AddDays(-1) && !r.ReturnDate.HasValue && r.IsOverdue)
                        .Union(_context.Rentals.Where(r => r.RentedOnDate >= lastMonthStart && r.RentedOnDate < currentMonthStart && !r.ReturnDate.HasValue && r.IsOverdue))
                        .Include(r => r.User) // Include related User data
                        .Include(r => r.Book) // Include related User data
                        .Select(r => new UserOverDueRentalDto
                        {
                            UserId = r.UserId,
                            Name = r.User.Name,
                            Email = r.User.Email,
                            RentalId = r.Id,
                            BookId = r.BookId,
                            BookTitle = r.Book.Title,
                            RentedOn = r.RentedOnDate,
                            ReturnedOn = r.ReturnDate,
                            IsOverdue = r.IsOverdue
                        })
                        .ToListAsync();

        }

        public async Task<List<RentalHistoryDto>> GetAllRentalHistoryAsync()
        {
            var rentalHistory = await _context.Rentals
               .Include(r => r.Book) // Include related Book data
               .Select(r => new RentalHistoryDto
               {
                   RentalId = r.Id,
                   BookId = r.BookId,
                   UserId = r.UserId,
                   BookTitle = r.Book.Title,
                   RentedOn = r.RentedOnDate,
                   ReturnedOn = r.ReturnDate,
                   IsOverdue = r.IsOverdue
               })
               .OrderByDescending(r => r.RentedOn) // Order by most recent rentals
               .ToListAsync();

            return rentalHistory;
        }
    }
}