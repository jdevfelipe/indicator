using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;

        public PaymentController(PaymentService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("add-new-payment")]
        [Authorize(Roles = "MANAGER")]
        public Payment NewPayment([FromBody] Payment payment, [FromHeader] int indication)
        {
            return _service.AddPayment(indication, payment);
        }

        [HttpPost]
        [Route("add-new-receipt")]
        [Authorize(Roles = "MANAGER")]
        public Task<Payment> NewReceipt([FromForm] IFormFile file, [FromHeader] int payment)
        {
            return _service.InsertReceiptAsync(file, payment);
        }

        [HttpGet]
        [Route("list-payments/{indicatorId}")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public PaymentPaginated GetPayments(int indicatorId, [FromHeader] String searchTerm, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.ListPayments(indicatorId, searchTerm, page, limit);
        }

        [HttpGet]
        [Route("get-payments-by-company")]
        [Authorize(Roles = "MANAGER, INDICATOR, ANALYST")]
        public PaymentPaginated GetPaymentsByCompanies([FromHeader] List<int> companiesIds, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetPaymentsByCompanies(companiesIds, page, limit);
        }

        [HttpGet]
        [Route("confirm-payment-receive")]
        [Authorize(Roles = "INDICATOR")]
        public Payment ConfirmReceive([FromHeader] int paymentId)
        {
            return _service.ConfirmReceive(paymentId);
        }

        
        [HttpGet]
        [Route("get-pay-paper")]
        [Authorize(Roles = "INDICATOR")]
        public Payment ShowPayment([FromHeader] int paymentId)
        {
            return _service.ShowPay(paymentId);
        }
    }
}
