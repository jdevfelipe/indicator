using indicator_api.Entities;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.MailSender
{
    public class SendPaymentMail
    {
        private readonly ILogger<Payment> _logger;
        public SendPaymentMail(ILogger<Payment> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendPaymentConfirmedAsync(List<EmailAddress> emailsTo)
        {
            var apiKey = Settings.Settings.SendgridKey;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("jdevfelipe@gmail.com", "SoftLuv"));

            var recipients = emailsTo;
            msg.AddTos(recipients);

            msg.SetSubject("Pagamento confirmado :)");

            msg.AddContent(MimeType.Text, "Olá :)");
            msg.AddContent(MimeType.Html, "<p>Percebemos que você confirmou nosso pagamento, agradecemos, e continue indicando!</p>");
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Send mail to notify, response is: {response.StatusCode}");
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted) 
            {
                return true;
            }
            return false;
        }
    }
}
