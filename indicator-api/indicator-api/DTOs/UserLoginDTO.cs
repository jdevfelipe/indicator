using indicator_api.Entities;
using indicator_api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class UserLoginDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Token { get; set; }
        public ICollection<UserCompany> UserCompanies { get; set; } = new List<UserCompany>();
        public UserEnum UserEnum { get; set; }
        public UserRole UserRole { get; set; }
    }
}
