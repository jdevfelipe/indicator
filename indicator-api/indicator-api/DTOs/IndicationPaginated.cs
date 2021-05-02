using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class IndicationPaginated
    {
        public List<Indication> indications { get; set; }
        public int total { get; set; }
    }
}
