using indicator_api.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace indicator_api.Entities
{
    [Table("companies")]
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string CorporateSocialName { get; set; }
        public string FantasyName { get; set; }
        public string CNPJ { get; set; }
        public string ImageURI { get; set; }
        public string obs { get; set; }
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public CompanyStatus Status { get; set; }
    }
}
