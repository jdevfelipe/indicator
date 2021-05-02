using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Utils
{
    interface UserUtil : SuperUtil
    {
        public Boolean NameIsEmty (User user)
        {
            if (StringIsNull(user.Name))
            {
                return true;
            }
            return false;
        }

        public Boolean EmailIsEmpty (User user)
        {
            if (StringIsNull(user.Email))
            {
                return true;
            }
            return false;
        }

        public Boolean CPFIsEmpty(User user)
        {
            if (StringIsNull(user.CPF))
            {
                return true;
            }
            return false;
        }
    }
}
