using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Repositories
{
    public class PaymentAccountRepository : RepositoryBase<PaymentAccount>, IPaymentAccount
    {
        public PaymentAccountRepository(AppDbContext Context) : base(Context)
        {

        }

        public PaymentAccount GetByAccount(String account) 
        {
            return Context.PaymentAccounts.FirstOrDefault(x => x.Account == account);
        }
    }
}
