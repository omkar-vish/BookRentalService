using BookRentalService.DTO;
using BookRentalService.Interface;
using BookRentalService.Model;
using BookRentalService.Repository;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BookRentalService.Service
{
    public class EmailService : IEmailService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ILogger<EmailService> _logger;
        private readonly AppSettings _appSettings;

        public EmailService(IRentalRepository rentalRepository, ILogger<EmailService> logger, IOptions<AppSettings> appSettings)
        {
            _rentalRepository = rentalRepository;
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task SendLastMonthOverdueNotificationAsync()
        {
            var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);

            var overdueRentals = await _rentalRepository.GetOverdueRentalsAsync(currentMonthStart, lastMonthStart);

            // Send email service. From above we get user email id and book details.

            for (int i = 0; i < overdueRentals.Count; i++)
            {
                await SendEmail(overdueRentals[i]);
            }

            _logger.LogInformation(" SendLastMonthOverdueNotificationAsync ");

        }

        public async Task SendEmail(UserOverDueRentalDto userOverDueRental)
        {
            try
            {
                var fromAddress = _appSettings.FromAddress;
                var fromPassword = _appSettings.FromPassword;
                string subject = _appSettings.Subject;
                string smtphost = _appSettings.SmtpHost;
                var toAddress = userOverDueRental.Email;

                using (var smtpclient = new SmtpClient())
                {
                    if (!string.IsNullOrEmpty(fromAddress) && !string.IsNullOrEmpty(fromPassword))
                    {
                        await smtpclient.ConnectAsync(smtphost, 465, true);
                        await smtpclient.AuthenticateAsync(fromAddress, fromPassword);
                    }
                    else
                    {
                        await smtpclient.ConnectAsync(smtphost, 25, false);
                    }

                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("BookRentalService", fromAddress));
                    email.To.Add(new MailboxAddress(userOverDueRental.Name, toAddress));
                    email.Subject = subject;
                    email.Body = new TextPart("plain")
                    {
                        Text = @$"Hey {userOverDueRental.Name}, 

                                 Rented book {userOverDueRental.BookTitle} on {userOverDueRental.RentedOn.ToShortDateString()} is Overdue  return date. 

                                 
                                Regards,
                                BookRentalService"
                    };

                    await smtpclient.SendAsync(email);
                    await smtpclient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
