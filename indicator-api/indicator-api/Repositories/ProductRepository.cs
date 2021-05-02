using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Repositories.Interfaces
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext Context) : base(Context)
        {

        }

        public List<Product> getAllProducts(int page, int limit) {
            return Context.Products.Include(x => x.Company).Include(x => x.Indications).Where(x => x.Status.Equals(Enums.ProductStatus.ACTIVE)).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public Product getProduct(int id) 
        {
            return Context.Products.Include(x => x.Company)
                                   .Include(x => x.Indications).Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Product> getProductsByCompaniesId(List<int> ids, int page, int limit)
        {
            return Context.Products.Include(x => x.Company)
                                   .Include(x => x.Indications).Where(x => ids.Contains(x.CompanyId)).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int countProductsByCompaniesId(List<int> ids)
        {
            return Context.Products.Include(x => x.Company)
                                   .Include(x => x.Indications).Where(x => ids.Contains(x.CompanyId)).Count();
        }

        public List<Product> getProductsByCompaniesId(List<int> ids)
        {
            return Context.Products.Include(x => x.Company)
                                   .Include(x => x.Indications).Where(x => ids.Contains(x.CompanyId)).ToList();
        }

        public List<Product> searchProducts(String term, int page, int limit) 
        {
            return Context.Products.Include(x => x.Company).Include(x => x.Indications).Where(x => x.name.ToLower().Contains(term.ToLower()) && x.Status.Equals(ProductStatus.ACTIVE)).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int countProducts() 
        {
            return Context.Products.Where(x => x.Status.Equals(ProductStatus.ACTIVE)).Count();
        }
    }
}
