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
    public class PaymentRepository : RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext Context) : base(Context)
        {

        }

        public List<Payment> ListPaymentsByUser(int indicatorId, int page, int limit) 
        {
            return Context.Payments.Include(x => x.Indication)
                .Include(x => x.Indication.Product).Where(x => x.Indication.IndicatorId == indicatorId).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public Payment GetById(int paymentId)
        {
            return Context.Payments.Include(x => x.Indication)
                .Include(x => x.Indication.Product).FirstOrDefault(x => x.Id == paymentId);
        }

        public List<Payment> GetPaymentsByCompanies(List<int> companiesIds, int page, int limit) 
        {
            return Context.Payments.Include(x => x.Indication.Indicator).Where(x => companiesIds.Contains(x.Indication.Product.CompanyId)).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int CountPaymentsByCompanies(List<int> companiesIds)
        {
            return Context.Payments.Include(x => x.Indication.Indicator).Where(x => companiesIds.Contains(x.Indication.Product.CompanyId)).Count();
        }

        public List<Payment> search(int id, String term, int page, int limit)
        {
            return Context.Payments.Include(x => x.Indication)
                .Include(x => x.Indication.Product).Where(x => x.Indication.IndicatorId == id && x.Indication.Product.name.ToLower().Contains(term.ToLower())).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int countPayments()
        {
            return Context.Payments.Count();
        }
    }
}
