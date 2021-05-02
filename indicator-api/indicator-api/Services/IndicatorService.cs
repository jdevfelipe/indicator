using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Exceptions;
using indicator_api.MailSender;
using indicator_api.Repositories;
using indicator_api.Utils;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class IndicatorService : IndicatorUtil
    {
        private readonly CpfValidate _cpfValidate;
        private readonly IndicatorRepository _repository;
        private readonly PaymentAccountRepository _paymentAccountRepository;
        private readonly SendConfirmAccountCode _sendConfirmMail;
        private readonly UserRepository _userRepository;
        private readonly SendForgotPassEmail _sendForgotPassEmail;

        public IndicatorService()
        {
        }

        public IndicatorService(CpfValidate cpfValidate, IndicatorRepository repository, PaymentAccountRepository paymentAccountRepository, SendConfirmAccountCode sendConfirmAccountCode, UserRepository userRepository, SendForgotPassEmail sendForgotPassEmail)
        {
            _cpfValidate = cpfValidate;
            _repository = repository;
            _paymentAccountRepository = paymentAccountRepository;
            _sendConfirmMail = sendConfirmAccountCode;
            _userRepository = userRepository;
            _sendForgotPassEmail = sendForgotPassEmail;
        }

        public IndicatorLoginDTO Login(string cpf, string password)
        {
            IndicatorUtil Util = new IndicatorService();

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

            Indicator indicator = _repository.GetIndicatorLogin(cpf, password);

            if (Util.ObjectIsNull(indicator))
            {
                throw new NotFoundException("CPF ou SENHA inválidos");
            }

            switch (indicator.Status)
            {
                case Enums.IndicatorStatus.INACTIVE:
                    throw new BadRequestException("Este usuário está inativo :c");
                case Enums.IndicatorStatus.BLOCKED:
                    throw new BadRequestException("Este usuário está bloqueado :c");
            }

            if (indicator.IsConfirmed.Equals(AccountConfirm.NOT_CONFIRMED))
            {
                throw new BadRequestException("NOT_CONFIRMED");
            }

            // Gera o Token para indicador
            var token = TokenService.GenerateTokenIndicator(indicator);

            IndicatorLoginDTO indicatorLogged = new IndicatorLoginDTO();
            indicatorLogged.Id = indicator.Id;
            indicatorLogged.CPF = indicator.CPF;
            indicatorLogged.Email = indicator.Email;
            indicatorLogged.Name = indicator.Name;
            indicatorLogged.Status = indicator.Status;
            indicatorLogged.PaymentAccounts = indicator.PaymentAccounts;
            indicatorLogged.Token = token;

            return indicatorLogged;
        }

        public IndicatorLoginDTO RegisterNewAsync(Indicator indicator)
        {
            IndicatorUtil Util = new IndicatorService();

            if (Util.CPFIsEmpty(indicator))
            {
                throw new BadRequestException("CPF deve ser preenchido");
            }

            if (_cpfValidate.ValidaCPF(indicator.CPF) == false)
            {
                throw new BadRequestException($"CPF inválido: {indicator.CPF}");
            }

            if (!Util.ObjectIsNull(_repository.GetIndicatorByCPF(indicator.CPF)))
            {
                throw new BadRequestException($"Já existe um usuário cadastrado com este cpf: {indicator.CPF}, faça login :)");
            }

            if (Util.StringIsNull(indicator.Password))
            {
                throw new BadRequestException("SENHA deve ser preenchida");
            }

            try
            {
                indicator.confirmCode = Util.GenerateRamdom();
                EmailAddress emailAddress = new EmailAddress();
                emailAddress.Email = indicator.Email;
                emailAddress.Name = indicator.Name;
                SendRegisterMailAsync(emailAddress, indicator.confirmCode).Wait();
                _repository.Create(indicator);
                IndicatorLoginDTO indicatorLogged = new IndicatorLoginDTO();
                indicatorLogged.CPF = indicator.CPF;
                indicatorLogged.Email = indicator.Email;
                indicatorLogged.Name = indicator.Name;
                indicatorLogged.Status = indicator.Status;
                indicatorLogged.IsConfirmed = indicator.IsConfirmed;
                indicatorLogged.ConfirmCode = indicator.confirmCode;
                return indicatorLogged;
            }
            catch (Exception e)
            {
                throw new Exception($"Tipo da excessão: {e.GetType()}");
            }
        }

        public PaymentAccount AddNewPaymentAccount(PaymentAccount paymentAccount)
        {
            IndicatorUtil Util = new IndicatorService();
            if (Util.BankIsEmpty(paymentAccount))
            {
                throw new BadRequestException("Banco deve estar preenchido");
            }

            if (Util.AgencyIsEmpty(paymentAccount))
            {
                throw new BadRequestException("Agência deve estar preenchida");
            }

            if (Util.AccountIsEmpty(paymentAccount))
            {
                throw new BadRequestException("Conta deve estar preenchida");
            }

            if (Util.ImageIsEmpty(paymentAccount))
            {
                throw new BadRequestException("Tire uma foto com documento");
            }

            try
            {
                _paymentAccountRepository.Create(paymentAccount);
                return paymentAccount;
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }
        }

        public IndicatorLoginDTO VerifyIfCodeIsOk(Indicator indicator, long code)
        {
            IndicatorUtil Util = new IndicatorService();
            Indicator indicator1 = _repository.GetIndicatorByCPF(indicator.CPF);

            if (Util.ObjectIsNull(indicator1))
            {
                throw new BadRequestException($"Nenhum usuário para este cpf: {indicator.CPF}, tente fazer login novamente");
            }

            if (code != indicator1.confirmCode)
            {
                throw new BadRequestException($"Código: {code} não confere, tente gerar um novo código ou confira se digitou corretamente");
            }
            try
            {
                // Gera o Token para indicador
                var token = TokenService.GenerateTokenIndicator(indicator);
                indicator1.IsConfirmed = AccountConfirm.CONFIRMED;
                _repository.Update(indicator1);
                IndicatorLoginDTO indicatorLogged = new IndicatorLoginDTO();
                indicatorLogged.CPF = indicator1.CPF;
                indicatorLogged.Id = indicator1.Id;
                indicatorLogged.Email = indicator1.Email;
                indicatorLogged.Name = indicator1.Name;
                indicatorLogged.Status = indicator1.Status;
                indicatorLogged.IsConfirmed = indicator1.IsConfirmed;
                indicatorLogged.ConfirmCode = indicator1.confirmCode;
                indicatorLogged.Token = token;
                return indicatorLogged;
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado, error: {e}");
            }

        }

        public async Task<bool> GenerateNewCodeAsync(Indicator indicator)
        {
            IndicatorUtil Util = new IndicatorService();
            Indicator indicator1 = _repository.GetIndicatorByCPF(indicator.CPF);

            if (Util.ObjectIsNull(indicator1))
            {
                throw new BadRequestException($"Nenhum usuário para este cpf: {indicator.CPF}, tente fazer login novamente");
            }

            indicator1.confirmCode = Util.GenerateRamdom();
            EmailAddress emailAddress = new EmailAddress();
            emailAddress.Email = indicator1.Email;
            emailAddress.Name = indicator1.Name;
            await _sendConfirmMail.SendCodeAsync(emailAddress, indicator1.confirmCode);
            _repository.Update(indicator1);

            return true;
        }

        public async Task<bool> SendRegisterMailAsync(EmailAddress emailAddress, long code)
        {
            return await _sendConfirmMail.SendCodeAsync(emailAddress, code);
        }

        public IndicatorLoginDTO UpdateProfile(IndicatorLoginDTO indicatorLogin)
        {
            if (indicatorLogin.Email.Length < 0)
            {
                throw new BadRequestException("Email deve estar preenchido");
            }

            if (indicatorLogin.Name.Length < 0)
            {
                throw new BadRequestException("Nome deve estar preenchido");
            }

            Indicator indicator = new Indicator();
            try 
            {
                indicator = _repository.GetIndicatorByCPF(indicatorLogin.CPF);
            } 
            catch (Exception e) 
            {
                throw new Exception($"Algo deu errado, error: {e}");
            }

            if (indicator == null) 
            {
                throw new NotFoundException("Nenhum usuário encontrado");
            }

            indicator.Email = indicatorLogin.Email;
            indicator.Name = indicatorLogin.Name;

            try 
            {
                _repository.Update(indicator);
            } 
            catch (Exception e) 
            {
                throw new Exception($"Algo deu errado, error: {e}");
            }

            return indicatorLogin;
        }

        public PaymentAccount UpdateBank(PaymentAccount paymentAccount)
        {
            if (paymentAccount.Account.Length < 1) 
            {
                throw new BadRequestException("CONTA não pode ser em branco");
            }

            if (paymentAccount.Bank.Length < 1)
            {
                throw new BadRequestException("BANCO não pode ser em branco");
            }

            if (paymentAccount.Agency.Length < 1)
            {
                throw new BadRequestException("AGÊNCIA não pode ser em branco");
            }

            PaymentAccount newPaymentAccount = new PaymentAccount();
            try
            {
                newPaymentAccount = _paymentAccountRepository.GetByAccount(paymentAccount.Account);
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado, error: {e}");
            }

            if (newPaymentAccount == null)
            {
                throw new NotFoundException("Nenhuma conta encontrado");
            }

            newPaymentAccount.Agency = paymentAccount.Agency;
            newPaymentAccount.Account = paymentAccount.Account;
            newPaymentAccount.Bank = paymentAccount.Bank;

            try
            {
                _paymentAccountRepository.Update(newPaymentAccount);
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado, error: {e}");
            }

            return newPaymentAccount;
        }

        public IndicatorLoginDTO GetById(int id) 
        {
            Indicator indicator = new Indicator();
            try 
            {
                indicator = _repository.GetIndicatorById(id);
            } catch (Exception e) 
            {
                throw new Exception("Algo deu errado");
            }

            if (indicator == null) 
            {
                throw new NotFoundException("Nenhum user encontrado");
            }


            // Gera o Token para indicador
            var token = TokenService.GenerateTokenIndicator(indicator);

            IndicatorLoginDTO indicatorLogged = new IndicatorLoginDTO();
            indicatorLogged.Id = indicator.Id;
            indicatorLogged.CPF = indicator.CPF;
            indicatorLogged.Email = indicator.Email;
            indicatorLogged.Name = indicator.Name;
            indicatorLogged.Status = indicator.Status;
            indicatorLogged.PaymentAccounts = indicator.PaymentAccounts;
            indicatorLogged.Token = token;

            return indicatorLogged;
        }

        public async Task<bool> forgotPassUser(String cpf)
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

            Indicator user = new Indicator();

            try
            {
                user = _repository.GetIndicatorByCPF(cpf);
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
