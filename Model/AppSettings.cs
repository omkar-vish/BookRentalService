namespace BookRentalService.Model
{
    public class AppSettings
    {
        public int MaxDaysRentDuration { get; set; }
        public string FromAddress { get; set; }
        public string FromPassword { get; set; }
        public string SmtpHost { get; set; }
        public string Subject { get; set; }
    }
}