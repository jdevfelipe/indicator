using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Password { get; set; }
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public UserEnum UserEnum { get; set; }
        public UserRole UserRole { get; set; }
    }
}
