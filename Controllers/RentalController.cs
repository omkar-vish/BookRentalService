using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookRentalService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalController> _logger;

        public RentalController(IRentalService rentalService, ILogger<RentalController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        [HttpGet("allhistory")]
        public async Task<IEnumerable<RentalHistoryDto>> GetAllRentalHistoryAsync()
        {
            _logger.LogInformation("GetAllRentalHistoryAsync called.");
            var rentals = await _rentalService.GetAllRentalHistoryAsync();
            return rentals.ToArray();
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentBook(int userId, int bookId)
        {
            _logger.LogInformation("RentBook called.");
            try
            {
                var rental = await _rentalService.RentBookAsync(userId, bookId);
                return Ok(rental);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RentBook error userId:{userId} bookId:{bookId}", userId, bookId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook(int rentalId)
        {
            _logger.LogInformation("ReturnBook called.");

            try
            {
                await _rentalService.ReturnBookAsync(rentalId);
                return Ok("Book returned successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnBook error rentalId:{rentalId}", rentalId);
                return BadRequest(ex.Message);
            }
        } 

    }
}
