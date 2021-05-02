using indicator_api.Context;
using indicator_api.Entities;
using indicator_api.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace indicator_api.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext Context) : base(Context)
        {

        }

        public User GetUserLogin(string cpf, string password) 
        {
            //return Context.Users.FromSqlRaw($"SELECT * FROM users WHERE CPF = {cpf} and password = {password}").FirstOrDefault();
            return Context.Users.Include(x => x.UserCompanies).FirstOrDefault(x => x.CPF.Equals(cpf) && x.Password.Equals(password));
        }

        public User GetUserByCPF(string cpf) 
        {
            //return Context.Users.FromSqlRaw($"SELECT * FROM users WHERE CPF = {cpf}").FirstOrDefault();
            return Context.Users.FirstOrDefault(x => x.CPF.Equals(cpf));
        }

        public User GetUserManagerByCPF(string cpf)
        {
            //return Context.Users.FromSqlRaw($"SELECT * FROM users WHERE CPF = {cpf}").FirstOrDefault();
            return Context.Users.FirstOrDefault(x => x.CPF.Equals(cpf) && x.UserRole.Equals(UserRole.MANAGER));
        }

        public List<User> GetUsers(int page, int limit) 
        {
            return Context.Users.Include(x => x.UserCompanies).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public User GetUserById(int id)
        {
            return Context.Users.First(x => x.Id == id);
        }

        public List<User> GetUsersByCompanies(List<int> ids, int page, int limit) 
        {
            return Context.Users.Include(x => x.UserCompanies).Where(y => y.UserCompanies.Any(z => ids.Contains(z.CompanyId))).Skip((page - 1) * limit)
               .Take(limit)
               .ToList();
        }

        public int CountUsersByCompanies(List<int> ids)
        {
            return Context.Users.Include(x => x.UserCompanies).Where(y => y.UserCompanies.Any(z => ids.Contains(z.CompanyId))).Count();
        }

        public int countUser()
        {
            return Context.Users.Count();
        }
    }
}
