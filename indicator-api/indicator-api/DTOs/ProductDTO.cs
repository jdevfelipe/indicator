using indicator_api.Entities;
using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string IndicationPrice { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Indication> Indications { get; set; } = new List<Indication>();
        public ProductStatus Status { get; set; }
    }
}
