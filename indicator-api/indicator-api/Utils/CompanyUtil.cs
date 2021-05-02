using indicator_api.DTOs;
using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Utils
{
    interface CompanyUtil : SuperUtil
    {
        public Boolean FantasyNameIsEmty(CompanyRegister company)
        {
            if (StringIsNull(company.FantasyName))
            {
                return true;
            }
            return false;
        }

        public Boolean CorporateSocialNameIsEmpty(CompanyRegister company)
        {
            if (StringIsNull(company.CorporateSocialName))
            {
                return true;
            }
            return false;
        }

        public Boolean CNPJIsEmpty(CompanyRegister company)
        {
            if (StringIsNull(company.CNPJ))
            {
                return true;
            }
            return false;
        }
    }
}
