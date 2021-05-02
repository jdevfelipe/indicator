using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("payment_accounts")]
    public class PaymentAccount
    {
        [Key]
        public int Id { get; set; }
        public string Bank { get; set; }
        public string Agency { get; set; }
        public string Account { get; set; }
        public string imageURI { get; set; }
        public int IndicatorId { get; set; }
        public Indicator Indicator { get; set; }
        public PaymentAccountStatus Status { get; set; }
    }
}
