using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Model;
using BookRentalService.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;
        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        // GET: api/<BookController>
        [HttpGet]
        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            _logger.LogInformation("GetAllBooks called.");
            var books = await _bookService.GetAllBooks();
            return books.ToArray();
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string name = null, string genre = null)
        {
            _logger.LogInformation("SearchBooks called.");
            try
            {
                var books = await _bookService.SearchBooks(name, genre);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchBooks name{name} genre{genre}", name, genre);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("stats")]
        public async Task<StatResult> GetBookStats()
        {
            _logger.LogInformation("GetBookStats called.");
            var stats = new StatResult
            {
                MostPopularBook = await _bookService.GetMostRentedBookAsync(),
                LeastPopularBook = await _bookService.GetLeastRentedBookAsync(),
                MostOverdueBook = await _bookService.GetMostOverdueBookAsync()
            };

            return stats;
        }
    }
}
