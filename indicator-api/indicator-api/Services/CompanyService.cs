using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Exceptions;
using indicator_api.MailSender;
using indicator_api.Repositories;
using indicator_api.Utils;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class CompanyService : CompanyUtil
    {
        private readonly CompanyRepository _repository;
        private readonly UserRepository _userRepository;
        private readonly CnpjValidate _cnpjValidate;
        private readonly ILogger<Company> _logger;
        private readonly SendForgotPassEmail _sendForgotPassEmail;

        public CompanyService()
        {
        }

        public CompanyService(CompanyRepository repository, CnpjValidate cnpjValidate, UserRepository userRepository, ILogger<Company> logger, SendForgotPassEmail sendForgotPassEmail)
        {
            _repository = repository;
            _cnpjValidate = cnpjValidate;
            _userRepository = userRepository;
            _logger = logger;
            _sendForgotPassEmail = sendForgotPassEmail;
        }

        public Company InsertNew(CompanyRegister company)
        {
            CompanyUtil util = new CompanyService();

            if (util.ObjectIsNull(company))
            {
                throw new BadRequestException("Empresa não pode estar vazia");
            }

            if (util.CNPJIsEmpty(company))
            {
                throw new BadRequestException("CNPJ não pode estar em branco");
            }

            if (!_cnpjValidate.ValidaCNPJ(company.CNPJ))
            {
                throw new BadRequestException($"CNPJ inválido: {company.CNPJ}");
            }

            if (util.CorporateSocialNameIsEmpty(company))
            {
                throw new BadRequestException("RAZÃO SOCIAL deve ser preenchida");
            }

            if (util.FantasyNameIsEmty(company))
            {
                throw new BadRequestException("Preencha NOME FANTASIA por favor!");
            }

            User user = _userRepository.GetUserManagerByCPF(company.CPF);

            if (user == null)
            {
                throw new BadRequestException("Não existe um administrador cadastrado com esse CPF");
            }

            UserCompany userCompany = new UserCompany();
            userCompany.UserId = user.Id;

            Company company1 = new Company();
            company1.CNPJ = company.CNPJ;
            company1.CorporateSocialName = company.CorporateSocialName;
            company1.FantasyName = company.FantasyName;
            userCompany.CompanyId = company1.Id;
            company1.UserCompanies.Add(userCompany);

            try
            {
                _repository.Create(company1);
                return company1;
            }
            catch (Exception e)
            {
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }
        }

        public CompanyPaginatedDTO GetCompanies(int page, int limit)
        {
            List<Company> companies = new List<Company>();
            CompanyPaginatedDTO companyPDTO = new CompanyPaginatedDTO();
            int total;
            try
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Getting companies");
                companies = _repository.GetCompanies(page, limit);
                total = _repository.CountComp();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Error on getting companies");
                throw new Exception($"Error is: {e.GetType()}");
            }

            if (companies.Count() < 1)
            {
                throw new NotFoundException("Não existem empresas cadastradas");
            }

            List<CompanyRegister> companyRegisters = new List<CompanyRegister>();

            foreach (Company c in companies)
            {
                CompanyRegister company = new CompanyRegister();
                company.CNPJ = c.CNPJ;
                company.CorporateSocialName = c.CorporateSocialName;
                company.FantasyName = c.FantasyName;
                company.Status = c.Status;


                List<UserCompany> usersCompanies = (List<UserCompany>)c.UserCompanies;
                foreach (UserCompany u in usersCompanies)
                {
                    u.User = _userRepository.GetUserById(u.UserId);
                }

                company.CPF = usersCompanies.First(x => x.User.UserRole == Enums.UserRole.MANAGER).User.CPF;

                companyRegisters.Add(company);
            }

            companyPDTO.companyRegisters = companyRegisters;
            companyPDTO.total = total;

            return companyPDTO;
        }

        public List<CompanyRegister> GetCompaniesById(List<int> ids)
        {
            if (ids.Count() < 1)
            {
                throw new BadRequestException("Nenhuma empresa cadastrada para este usuário");
            }

            List<Company> companies = new List<Company>();

            try
            {
                companies = _repository.GetCompaniesById(ids);
            }
            catch (Exception e)
            {
                throw new Exception("Algo deu errado: " + e.GetType());
            }


            if (companies.Count() < 1)
            {
                throw new NotFoundException("Nenhuma empresa cadastrada para este usuário");
            }

            List<CompanyRegister> companyRegisters = new List<CompanyRegister>();

            foreach (Company c in companies)
            {
                CompanyRegister companyRegister = new CompanyRegister();
                companyRegister.Id = c.Id;
                companyRegister.FantasyName = c.FantasyName;
                companyRegister.CorporateSocialName = c.CorporateSocialName;
                companyRegister.CNPJ = c.CNPJ;
                companyRegister.Status = c.Status;

                companyRegisters.Add(companyRegister);
            }

            return companyRegisters;
        }

        public UserRegisterDTO UpdateUser(User user)
        {

            try
            {
                User user1 = _userRepository.GetUserByCPF(user.CPF);

                user1.CPF = user.CPF;
                user1.Email = user.Email;
                user1.Name = user.Name;
                user1.UserEnum = user.UserEnum;
                user1.UserRole = user.UserRole;

                _userRepository.Update(user1);
            }
            catch (Exception e)
            {
                throw new Exception("Algo deu errado: " + e.GetType());
            }

            UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
            userRegisterDTO.CPF = user.CPF;
            userRegisterDTO.Email = user.Email;
            userRegisterDTO.Name = user.Name;
            userRegisterDTO.UserEnum = user.UserEnum;
            userRegisterDTO.UserRole = user.UserRole;
            userRegisterDTO.Id = user.Id;

            return userRegisterDTO;
        }

        public UserRegisterDTO DeleteUser(int id)
        {

            if (id < 1)
            {
                throw new BadRequestException("Id nullo");
            }
            try
            {
                User user = _userRepository.GetUserById(id);

                user.UserEnum = Enums.UserEnum.INACTIVE;

                _userRepository.Update(user);

                UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
                userRegisterDTO.CPF = user.CPF;
                userRegisterDTO.Email = user.Email;
                userRegisterDTO.Name = user.Name;
                userRegisterDTO.UserEnum = user.UserEnum;
                userRegisterDTO.UserRole = user.UserRole;
                userRegisterDTO.Id = user.Id;

                return userRegisterDTO;
            }
            catch (Exception e)
            {
                throw new Exception("Algo deu errado: " + e.GetType());
            }
        }

        public Company UpdateCompany(Company company)
        {

            try
            {
                Company company1 = _repository.GetCompaniesByDoc(company.CNPJ);

                company1.CNPJ = company.CNPJ;
                company1.CorporateSocialName = company.CorporateSocialName;
                company1.FantasyName = company.FantasyName;
                company1.Status = company.Status;

                _repository.Update(company1);
            }
            catch (Exception e)
            {
                throw new Exception("Algo deu errado: " + e.GetType());
            }

            return company;
        }

        public async Task<bool> ForgotPassAsync(String cpf)
        {
            CpfValidate cpfValidate = new CpfValidate();

            if (String.IsNullOrEmpty(cpf))
            {
                throw new BadRequestException("CPF em branco!");
            }

            if (!cpfValidate.ValidaCPF(cpf)) 
            {
                throw new BadRequestException("CPF inválido!");
            }

            User user = new User();

            try
            {
                user = _userRepository.GetUserByCPF(cpf);
            }
            catch (Exception e)
            {
                throw new Exception("Algo deu errado: " + e.GetType());
            }

            if (user == null) 
            {
                throw new NotFoundException("Não existe um usuário com este documento!");
            }

            bool res = await _sendForgotPassEmail.SendMessageAsync(new EmailAddress(user.Email, user.Name), "Sua senha é: " + user.Password);

            return res;
        }
    }
}
