using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Exceptions;
using indicator_api.MailSender;
using indicator_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace indicator_api.Jobs
{
    public class NewPaymentJob : CronJobService
    {
        private readonly ILogger<NewPaymentJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SendPaymentMail _sendPaymentMail;

        public NewPaymentJob(IScheduleConfig<NewPaymentJob> config, ILogger<NewPaymentJob> logger, IServiceScopeFactory serviceScopeFactory, SendPaymentMail sendPaymentMail)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _sendPaymentMail = sendPaymentMail;
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Notify payments runned");
                List<Payment> payments = dbContext.Payments.Include(x => x.Indication)
                                                           .Include(x => x.Indication.Indicator).Where(x => x.Status.Equals(PaymentStatus.CONFIRMED)).ToList();

                if (payments.Count < 1)
                {
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} No has payments to notify");
                    return Task.CompletedTask;
                }

                List<EmailAddress> emailAddresses = new List<EmailAddress>();

                foreach (Payment p in payments)
                {
                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Email = p.Indication.Indicator.Email;
                    emailAddress.Name = p.Indication.Indicator.Name;
                    
                    emailAddresses.Add(emailAddress);

                }

                if (_sendPaymentMail.SendPaymentConfirmedAsync(emailAddresses).Result)
                {
                    foreach (Payment p in payments) 
                    {
                        p.Status = PaymentStatus.CONFIRMED_AND_SENDED;
                        dbContext.Entry(p).State = EntityState.Modified;
                        dbContext.SaveChanges();
                    }
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Payment Notified by email");
                    return Task.CompletedTask;
                }
                else {
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Error on send mail");
                    return Task.FromResult("Error on send mail");
                }
            }
        }
    }
}
