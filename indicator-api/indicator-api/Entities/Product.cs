using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public Double IndicationPrice { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public string Obs { get; set; }
        public ICollection<Indication> Indications { get; set; } = new List<Indication>();
        public ProductStatus Status { get; set; }
    }
}
