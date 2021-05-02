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
    public class SendConfirmAccountCode
    {
        private readonly ILogger<Indicator> _logger;
        public SendConfirmAccountCode(ILogger<Indicator> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendCodeAsync(EmailAddress emailTo, long confirmCode)
        {
            var apiKey = Settings.Settings.SendgridKey;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("jdevfelipe@gmail.com", "Indicator"));

            var recipient = emailTo;
            msg.AddTo(recipient);

            msg.SetSubject($"Código de confirmação - {confirmCode}");

            msg.AddContent(MimeType.Text, "Olá :)");
            msg.AddContent(MimeType.Html, "<p>Oi, você se cadastrou em nossa plataforma ?</br> " + $"seu código de confirmação é: {confirmCode}</p>");
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
