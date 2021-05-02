using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("indications")]
    public class Indication
    {
        [Key]
        public int Id { get; set; }
        public DateTime IndicationDate { get; set; } = DateTime.Now;
        public string Document { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Observation { get; set; }
        public DocumentType DocumentType { get; set; }
        public BranchType Branch { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public int IndicatorId { get; set; }
        public Indicator Indicator { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string obs { get; set; }
        public IndicationStatus Status { get; set; }
    }
}
