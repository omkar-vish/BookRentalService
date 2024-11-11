namespace BookRentalService.Interface
{
    public interface IEmailService
    {
        Task SendLastMonthOverdueNotificationAsync();
    }
}