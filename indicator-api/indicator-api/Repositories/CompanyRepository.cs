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
    public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext Context) : base(Context)
        {

        }

        public List<Company> GetCompanies(int page, int limit) 
        {
            return Context.Companies.Include(x => x.UserCompanies).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public List<Company> GetCompaniesById(List<int> ids)
        {
            return Context.Companies.Where(x => ids.Contains(x.Id)).ToList();
        }

        public int CountComp() 
        {
            return Context.Companies.Count();
        }

        public Company GetCompaniesByDoc(string doc)
        {
            return Context.Companies.FirstOrDefault(x => x.CNPJ.Equals(doc));
        }
    }
}
