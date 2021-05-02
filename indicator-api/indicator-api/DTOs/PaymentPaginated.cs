using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class PaymentPaginated
    {
        public List<Payment> payments { get; set; }
        public int total { get; set; }
    }
}
