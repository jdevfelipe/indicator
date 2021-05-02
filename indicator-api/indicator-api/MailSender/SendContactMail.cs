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
    public class SendContactMail
    {
        private readonly ILogger<Indicator> _logger;
        public SendContactMail(ILogger<Indicator> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendMessageAsync(EmailAddress emailTo, String message)
        {
            var apiKey = Settings.Settings.SendgridKey;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("jdevfelipe@gmail.com", "Indicator"));

            var recipient = emailTo;
            msg.AddTo(recipient);

            msg.SetSubject("Contato!");

            msg.AddContent(MimeType.Text, "Olá :)");
            msg.AddContent(MimeType.Html, "<p>Você recebeu um contato!!!</br></p>" + "<p>" + message + "</p>");
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
