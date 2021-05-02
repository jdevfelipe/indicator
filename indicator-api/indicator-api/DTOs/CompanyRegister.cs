using indicator_api.Entities;
using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class CompanyRegister
    {
        public int Id { get; set; }
        public string CorporateSocialName { get; set; }
        public string FantasyName { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public User UserManager { get; set; }
        public string ImageURI { get; set; }
        public CompanyStatus Status { get; set; }
    }
}
