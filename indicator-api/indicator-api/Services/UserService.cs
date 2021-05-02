using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Exceptions;
using indicator_api.Repositories;
using indicator_api.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class UserService : UserUtil
    {
        private readonly UserRepository _repository;
        private readonly CpfValidate _cpfValidate;
        private readonly ILogger<User> _logger;

        public object UsersLoginDTO { get; private set; }

        public UserService()
        {
        }

        public UserService(UserRepository repository, CpfValidate cpfValidate, ILogger<User> logger)
        {
            _repository = repository;
            _cpfValidate = cpfValidate;
            _logger = logger;
        }

        public UserLoginDTO Login(string cpf, string password)
        {
            UserUtil Util = new UserService();

            if (Util.StringIsNull(cpf))
            {
                throw new BadRequestException("CPF deve ser preenchido");
            }

            if (Util.StringIsNull(password))
            {
                throw new BadRequestException("SENHA deve ser preenchida");
            }

            if (_cpfValidate.ValidaCPF(cpf) == false)
            {
                throw new BadRequestException($"CPF inválido: {cpf}");
            }

            User user = _repository.GetUserLogin(cpf, password);

            if (Util.ObjectIsNull(user))
            {
                throw new NotFoundException("CPF ou SENHA inválidos");
            }

            switch (user.UserEnum)
            {
                case Enums.UserEnum.INACTIVE:
                    throw new BadRequestException("Este usuário está inativo :c");
                case Enums.UserEnum.BLOCKED:
                    throw new BadRequestException("Este usuário está bloqueado :c");
            }

            // Gera o Token
            var token = TokenService.GenerateTokenUserCompany(user);

            UserLoginDTO userLogged = new UserLoginDTO();
            userLogged.CPF = user.CPF;
            userLogged.Email = user.Email;
            userLogged.Name = user.Name;
            userLogged.UserCompanies = user.UserCompanies;
            userLogged.UserEnum = user.UserEnum;
            userLogged.UserRole = user.UserRole;
            userLogged.Token = token;

            return userLogged;
        }

        public UserLoginDTO RegisterNew(User user)
        {
            UserUtil Util = new UserService();

            if (Util.ObjectIsNull(user))
            {
                throw new NotFoundException("Usuário não está preenchido!");
            }

            if (!Util.ObjectIsNull(_repository.GetUserByCPF(user.CPF)))
            {
                throw new BadRequestException($"Já existe um usuário cadastrado com este cpf: {user.CPF}, faça login :)");
            }

            if (Util.NameIsEmty(user))
            {
                throw new BadRequestException($"Nome deve ser preenchido {user.Name}");
            }

            if (Util.EmailIsEmpty(user))
            {
                throw new BadRequestException($"Email deve ser preenchido {user.Email}");
            }

            if (Util.StringIsNull(user.Password))
            {
                throw new BadRequestException("SENHA deve ser preenchida");
            }

            if (Util.CPFIsEmpty(user))
            {
                throw new BadRequestException("CPF deve ser preenchido");
            }

            if (_cpfValidate.ValidaCPF(user.CPF) == false)
            {
                throw new BadRequestException($"CPF inválido: {user.CPF}");
            }

            try
            {
                _repository.Create(user);
                UserLoginDTO userLogged = new UserLoginDTO();
                userLogged.CPF = user.CPF;
                userLogged.Email = user.Email;
                userLogged.Name = user.Name;
                userLogged.UserCompanies = user.UserCompanies;
                userLogged.UserEnum = user.UserEnum;
                userLogged.UserRole = user.UserRole;
                return userLogged;
            }
            catch (Exception e)
            {
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }
        }

        public UserRegisterDTO InsertNewUser(User user, List<int> companiesId) 
        {
            UserUtil Util = new UserService();

            if (companiesId.Count() < 1) 
            {
                throw new NotFoundException("Preencha o campo EMPRESA");
            }

            if (Util.ObjectIsNull(user))
            {
                throw new NotFoundException("Usuário não está preenchido!");
            }

            if (!Util.ObjectIsNull(_repository.GetUserByCPF(user.CPF)))
            {
                throw new BadRequestException($"Já existe um usuário cadastrado com este cpf: {user.CPF}, faça login :)");
            }

            if (Util.NameIsEmty(user))
            {
                throw new BadRequestException($"Nome deve ser preenchido {user.Name}");
            }

            if (Util.EmailIsEmpty(user))
            {
                throw new BadRequestException($"Email deve ser preenchido {user.Email}");
            }

            if (Util.StringIsNull(user.Password))
            {
                throw new BadRequestException("SENHA deve ser preenchida");
            }

            if (Util.CPFIsEmpty(user))
            {
                throw new BadRequestException("CPF deve ser preenchido");
            }

            try
            {
                List<UserCompany> usersCompany = new List<UserCompany>();
                UserRegisterDTO userDTO = new UserRegisterDTO();

                foreach (int id in companiesId)
                {
                    UserCompany userCompany = new UserCompany();
                    userCompany.CompanyId = id;

                    usersCompany.Add(userCompany);
                }

                user.UserCompanies = usersCompany;
                _repository.Create(user);

                userDTO.UserCompanies = usersCompany;
                userDTO.CPF = user.CPF;
                userDTO.Email = user.Email;
                userDTO.Name = user.Name;
                userDTO.UserEnum = user.UserEnum;
                userDTO.UserRole = user.UserRole;
                
                return userDTO;
            }
            catch (Exception e)
            {
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }


        }

        public UserPaginated GetUsers(int page, int limit)
        {
            List<User> users = new List<User>();
            UserPaginated userPaginated = new UserPaginated();
            try
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Getting users");
                users = _repository.GetUsers(page, limit);
                int total = _repository.countUser();
                userPaginated.total = total;
                
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Error on get users in db: {e}");
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }

            if (users.Count() < 1)
            {
                throw new NotFoundException("Não existem usuário cadastrados!");
            }

            List<UserLoginDTO> userLoginDTOs = new List<UserLoginDTO>();
            
            foreach (User u in users)
            {
                UserLoginDTO userLoginDTO = new UserLoginDTO();
                userLoginDTO.CPF = u.CPF;
                userLoginDTO.Email = u.Email;
                userLoginDTO.Name = u.Name;
                userLoginDTO.UserEnum = u.UserEnum;
                userLoginDTO.UserRole = u.UserRole;
                userLoginDTO.Id = u.Id;
                userLoginDTOs.Add(userLoginDTO);
            }
            userPaginated.userLoginDTOs = userLoginDTOs;
            return userPaginated;
        }

        public UsersPaginated GetUsersByCompanies(List<int> companies, int page, int limit)
        {
            if (companies.Count < 1) 
            {
                throw new BadRequestException("Algo deu errado, tente atualizar a página ou entrar em contato !");
            }
            List<User> users = new List<User>();
            try
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Getting users");
                users = _repository.GetUsersByCompanies(companies, page, limit);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} Error on get users in db: {e}");
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }

            if (users.Count() < 1)
            {
                throw new NotFoundException("Não existem usuário cadastrados!");
            }

            List<UserRegisterDTO> userRegisterDTOs = new List<UserRegisterDTO>();

            foreach (User u in users)
            {
                UserRegisterDTO userRegisterDTO = new UserRegisterDTO();
                userRegisterDTO.CPF = u.CPF;
                userRegisterDTO.Email = u.Email;
                userRegisterDTO.Name = u.Name;
                userRegisterDTO.UserEnum = u.UserEnum;
                userRegisterDTO.UserRole = u.UserRole;
                userRegisterDTO.UserCompanies = u.UserCompanies;
                userRegisterDTOs.Add(userRegisterDTO);
            }

            UsersPaginated usersPaginated = new UsersPaginated();

            usersPaginated.users = userRegisterDTOs;
            usersPaginated.total = _repository.CountUsersByCompanies(companies);

            return usersPaginated;
        }
    }
}
