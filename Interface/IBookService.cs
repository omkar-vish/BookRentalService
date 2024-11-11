using BookRentalService.Model;

namespace BookRentalService.Interface
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetLeastRentedBookAsync();
        Task<Book> GetMostOverdueBookAsync();
        Task<Book> GetMostRentedBookAsync();
        Task<IEnumerable<Book>> SearchBooks(string name = null, string genre = null);
    }
}