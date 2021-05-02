using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class UsersPaginated
    {
        public List<UserRegisterDTO> users { get; set; }
        public int total { get; set; }
    }
}
