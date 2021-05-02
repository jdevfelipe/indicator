using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Repositories
{
    public class IndicationRepository : RepositoryBase<Indication>, IIndicationRepository
    {
        public IndicationRepository(AppDbContext Context) : base(Context)
        {

        }

        public List<Indication> GetIndications(int id, int page, int limit) 
        {
            return Context.Indications.Include(x => x.Product).Where(x => x.IndicatorId == id).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public Indication GetById(int id) 
        {
            return Context.Indications.FirstOrDefault(x => x.Id == id);
        }

        public List<Indication> GetByProductIds(List<int> ids, DateTime initialDate, DateTime finishDate)
        {
            return Context.Indications.Include(x => x.Product)
                .Include(x => x.Indicator)
                .Include(x => x.Payments)
                .Where(x => ids.Contains(x.ProductId) && x.IndicationDate >= initialDate && x.IndicationDate <= finishDate).ToList();
        }

        public List<Indication> search(int id, String term, int page, int limit)
        {
            return Context.Indications.Include(x => x.Product).Where(x => x.Product.name.Contains(term) && x.IndicatorId == id).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int countProducts()
        {
            return Context.Indications.Count();
        }
    }
}
