﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.DTOs
{
    public class UserPaginated
    {
        public List<UserLoginDTO> userLoginDTOs { get; set; }
        public int total { get; set; }
    }
}
