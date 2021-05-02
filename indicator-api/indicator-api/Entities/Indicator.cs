using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("indicators")]
    public class Indicator
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Password { get; set; }
        public long confirmCode { get; set; }
        public ICollection<Indication> Indications { get; set; } = new List<Indication>();
        public ICollection<PaymentAccount> PaymentAccounts { get; set; } = new List<PaymentAccount>();
        public AccountConfirm IsConfirmed { get; set; }
        public IndicatorRole IndicatorRole { get; set; }
        public IndicatorStatus Status { get; set; }
    }
}
