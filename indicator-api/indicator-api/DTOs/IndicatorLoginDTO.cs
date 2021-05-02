using indicator_api.Entities;
using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class IndicatorLoginDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Token { get; set; }
        public ICollection<Indication> Indications { get; set; } = new List<Indication>();
        public long ConfirmCode { get; set; }
        public AccountConfirm IsConfirmed { get; set; }
        public ICollection<PaymentAccount> PaymentAccounts { get; set; } = new List<PaymentAccount>();
        public IndicatorStatus Status { get; set; }
    }
}
