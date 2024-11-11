using BookRentalService.Interface;
using Hangfire;

public class SendNotificationOfOverDueRecurringJob
{
    public const string JobId = nameof(MarkOverDueBookRentalsRecurringJob);

    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<MarkOverDueBookRentalsRecurringJob> _logger;
    private readonly IEmailService _emailService;

    public SendNotificationOfOverDueRecurringJob(IBackgroundJobClient backgroundJobClient,
                                ILogger<MarkOverDueBookRentalsRecurringJob> logger, IEmailService emailService)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task ExecuteSendNotificationOfOverDueAsync()
    {
        _logger.LogInformation("Started Running recurring job {JobId} at {now}", JobId, DateTime.UtcNow);

        await _emailService.SendLastMonthOverdueNotificationAsync();

        _logger.LogInformation("Completed Running recurring job {JobId} at {now}", JobId, DateTime.UtcNow);

    }
}