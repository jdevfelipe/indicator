using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ImageURIOne { get; set; }
        public string ImageURITwo { get; set; }
        public DateTime ConfirmPaymentDate { get; set; }
        public int IndicationId { get; set; }
        public Indication Indication { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
