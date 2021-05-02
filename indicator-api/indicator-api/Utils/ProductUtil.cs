using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Utils
{
    interface ProductUtil : SuperUtil
    {
        public Boolean NameIsEmty(Product product)
        {
            if (StringIsNull(product.name))
            {
                return true;
            }
            return false;
        }

        public Boolean PriceIsEmty(Product product)
        {
            if (product.IndicationPrice <= 0)
            {
                return true;
            }
            return false;
        }

        public Boolean CompanyIsEmty(Product product)
        {
            if (product.CompanyId == 0)
            {
                return true;
            }
            return false;
        }
    }
}
