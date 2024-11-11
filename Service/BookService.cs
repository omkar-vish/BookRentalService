using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Model;
using BookRentalService.Repository;

namespace BookRentalService.Service
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRentalRepository _rentalRepository;

        public BookService(IBookRepository bookRepository, IRentalRepository rentalRepository)
        {
            _bookRepository = bookRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _bookRepository.GetAllBookAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooks(string name = null, string genre = null)
        {
            return await _bookRepository.Search(name, genre);
        }
        public async Task<Book> GetMostRentedBookAsync()
        {
            return await _bookRepository.GetMostRentedBookAsync();
        }
        public async Task<Book> GetMostOverdueBookAsync()
        {
            return await _bookRepository.GetMostOverdueBookAsync();
        }
        public async Task<Book> GetLeastRentedBookAsync()
        {
            return await _bookRepository.GetLeastRentedBookAsync();
        }
    }
}