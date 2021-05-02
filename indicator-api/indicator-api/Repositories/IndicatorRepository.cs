using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Repositories
{
    public class IndicatorRepository : RepositoryBase<Indicator>, IIndicatorRepository
    {
        public IndicatorRepository(AppDbContext Context) : base(Context)
        {

        }

        public Indicator GetIndicatorLogin(string cpf, string password)
        {
            //return Context.Indicators.FromSqlRaw($"SELECT * FROM indicators ind WHERE ind.CPF = '{cpf}' and ind.password = '{password}'").FirstOrDefault();
            return Context.Indicators.Include(x => x.PaymentAccounts)
                                     .FirstOrDefault(x => x.CPF == cpf && x.Password == password);
        }

        public Indicator GetIndicatorByCPF(string cpf) 
        {
            return Context.Indicators.FromSqlRaw($"SELECT * FROM indicators ind WHERE ind.CPF = {cpf}").FirstOrDefault();
        }

        public Indicator GetIndicatorById(int id)
        {
            return Context.Indicators.Include(x => x.PaymentAccounts).FirstOrDefault(x => x.Id == id);
        }
    }
}
