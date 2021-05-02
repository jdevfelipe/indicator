using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Exceptions;
using indicator_api.MailSender;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class IndicatorController : ControllerBase
    {
        private readonly IndicatorService _service;
        private readonly SendContactMail _sendContactMail;

        public IndicatorController(IndicatorService service, SendContactMail sendContactMail)
        {
            _service = service;
            _sendContactMail = sendContactMail;
        }


        // POST v1/<Company>
        [HttpPost]
        [Route("paymentaccount/add-new")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public PaymentAccount Post([FromBody] PaymentAccount paymentAccount)
        {
            return _service.AddNewPaymentAccount(paymentAccount);
        }

        // POST v1/<Company>
        [HttpPost]
        [Route("sendmail/send-new")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public async Task<MailContactDTO> SendNewMailAsync([FromBody] MailContactDTO mailContactDTO)
        {
            bool res = await _sendContactMail.SendMessageAsync(mailContactDTO, mailContactDTO.message);
            if (res == true)
            {
                return mailContactDTO;
            }
            else
            {
                throw new BadRequestException("Algo deu errado, tente mais tarde!");
            }

        }

        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public IndicatorLoginDTO UpdateProfile([FromBody] IndicatorLoginDTO indicatorLogin)
        {
            return _service.UpdateProfile(indicatorLogin);
        }

        [HttpPost]
        [Route("bank/update")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public PaymentAccount UpdateBank([FromBody] PaymentAccount paymentAccount)
        {
            return _service.UpdateBank(paymentAccount);
        }

        [HttpGet]
        [Route("update-profile")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public IndicatorLoginDTO UpdateProfile([FromHeader] int id)
        {
            return _service.GetById(id);
        }

        // GET: v1/<UserController>
        [HttpGet]
        [Route("forgot-pass")]
        [AllowAnonymous]
        public Task<bool> ForgotPass([FromHeader] string document)
        {
            return _service.forgotPassUser(document);
        }
    }
}
