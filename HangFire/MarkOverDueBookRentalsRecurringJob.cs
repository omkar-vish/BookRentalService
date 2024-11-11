using BookRentalService.Interface;
using Hangfire;

public class MarkOverDueBookRentalsRecurringJob
{
    public const string JobId = nameof(MarkOverDueBookRentalsRecurringJob);

    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<MarkOverDueBookRentalsRecurringJob> _logger;
    private readonly IRentalService _rentalService;

    public MarkOverDueBookRentalsRecurringJob(IBackgroundJobClient backgroundJobClient,
                                ILogger<MarkOverDueBookRentalsRecurringJob> logger, IRentalService rentalService)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
        _rentalService = rentalService;
    }

    public async Task ExecuteMarkOverdueRentalsAsync()
    {
        _logger.LogInformation("Started Running recurring job {JobId} at {now}", JobId, DateTime.UtcNow);

        await _rentalService.MarkOverdueRentalsAsync();

        _logger.LogInformation("Completed Running recurring job {JobId} at {now}", JobId, DateTime.UtcNow);

    }
}
