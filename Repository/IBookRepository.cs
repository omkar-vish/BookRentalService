using BookRentalService.Model;

namespace BookRentalService.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBookAsync();
        Task<Book> GetByIdAsync(int bookId);
        Task<Book> GetLeastRentedBookAsync();
        Task<Book> GetMostOverdueBookAsync();
        Task<Book> GetMostRentedBookAsync();
        Task<IEnumerable<Book>> Search(string name = null, string genre = null);
        Task UpdateAsync(Book book);
    }
}