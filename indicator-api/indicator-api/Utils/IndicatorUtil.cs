using indicator_api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Utils
{
    interface IndicatorUtil : SuperUtil
    {
        public Boolean NameIsEmty(Indicator indicator)
        {
            if (StringIsNull(indicator.Name))
            {
                return true;
            }
            return false;
        }

        public Boolean EmailIsEmpty(Indicator indicator)
        {
            if (StringIsNull(indicator.Email))
            {
                return true;
            }
            return false;
        }

        public Boolean CPFIsEmpty(Indicator indicator)
        {
            if (StringIsNull(indicator.CPF))
            {
                return true;
            }
            return false;
        }

        public Boolean BankIsEmpty(PaymentAccount paymentAccount)
        {
            if (StringIsNull(paymentAccount.Bank))
            {
                return true;
            }
            return false;
        }

        public Boolean AgencyIsEmpty(PaymentAccount paymentAccount)
        {
            if (StringIsNull(paymentAccount.Agency))
            {
                return true;
            }
            return false;
        }

        public Boolean AccountIsEmpty(PaymentAccount paymentAccount)
        {
            if (StringIsNull(paymentAccount.Account))
            {
                return true;
            }
            return false;
        }

        public Boolean ImageIsEmpty(PaymentAccount paymentAccount)
        {
            if (StringIsNull(paymentAccount.imageURI))
            {
                return true;
            }
            return false;
        }

        public Boolean IndicatorExists(PaymentAccount paymentAccount)
        {
            if (paymentAccount.IndicatorId == 0)
            {
                return true;
            }
            return false;
        }
    }
}
