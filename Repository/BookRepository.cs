using BookRentalService.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BookRentalService.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookRentalDbContext _context;

        public BookRepository(BookRentalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBookAsync()
        {
            return await _context.Books.ToArrayAsync();
        }

        public async Task<Book> GetByIdAsync(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task<Book> GetLeastRentedBookAsync()
        {
            return await _context.Books
                   .OrderBy(b => b.TimesRented)
                   .FirstOrDefaultAsync();
        }

        public async Task<Book> GetMostOverdueBookAsync()
        {
            // First, find the book with the most overdue rentals
            var mostOverdueBookId = await _context.Rentals
                                    .Where(r => r.IsOverdue)
                                    .GroupBy(r => r.BookId)
                                    .OrderByDescending(g => g.Count())
                                    .Select(g => g.Key)
                                    .FirstOrDefaultAsync();

            // Then, get the details of that book
            return await _context.Books
                         .FirstOrDefaultAsync(b => b.Id == mostOverdueBookId);
        }

        public async Task<Book> GetMostRentedBookAsync()
        {
            return await _context.Books
                         .OrderByDescending(b => b.TimesRented)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> Search(string name = null, string genre = null)
        {
            return await _context.Books.Where(book => (book.Title.Equals(name) || book.Genre.Equals(genre))).ToArrayAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}