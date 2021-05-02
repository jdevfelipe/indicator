using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class MailContactDTO : EmailAddress
    {
        public string message { get; set; }
    }
}
