using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Exceptions;
using indicator_api.Repositories;
using indicator_api.Repositories.Interfaces;
using indicator_api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class IndicationService : IndicationUtil
    {
        private readonly IndicationRepository _repository;
        private readonly ProductRepository _productRepository;
        private readonly UserRepository _userRepository;
        private readonly CnpjValidate _cnpjValidate;
        private readonly CpfValidate _cpfValidate;

        public IndicationService()
        {
        }

        public IndicationService(IndicationRepository repository, ProductRepository productRepository, UserRepository userRepository, CnpjValidate cnpjValidate, CpfValidate cpfValidate)
        {
            _repository = repository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _cnpjValidate = cnpjValidate;
            _cpfValidate = cpfValidate;
        }

        public Indication InsertNew(Indication indication) {
            IndicationUtil util = new IndicationService();

            if (util.ObjectIsNull(indication))
            {
                throw new BadRequestException($"Indicação não pode estar vazia!");
            }

            if (util.StringIsNull(indication.DocumentType.ToString())) 
            {
                throw new BadRequestException($"Preencha o tipo da indicação!");
            }

            if (util.StringIsNull(indication.Document)) 
            {
                throw new BadRequestException($"Documento da indicação deve estar preenchida");
            }

            switch (indication.DocumentType)
            {
                case Enums.DocumentType.CNPJ:
                    if (!_cnpjValidate.ValidaCNPJ(indication.Document))
                    {
                        throw new BadRequestException($"Este cnpj é inválido: {indication.Document}");
                    }
                    break;
                case Enums.DocumentType.CPF:
                    if (!_cpfValidate.ValidaCPF(indication.Document))
                    {
                        throw new BadRequestException($"Este cpf é inválido: {indication.Document}");
                    }
                    break;
            }

            if (util.StringIsNull(indication.CellPhone) && util.StringIsNull(indication.Phone))
            {
                throw new BadRequestException($"Preencha pelo menos um dos telefones!");
            }

            if (util.StringIsNull(indication.Branch.ToString()))
            {
                throw new BadRequestException($"O ramo de atuação da indicação deve ser preenchido");
            }

            Product product = _productRepository.getProduct(indication.ProductId);

            foreach (Indication i in product.Indications) 
            {
                if (i.Document == indication.Document) 
                {
                    throw new BadRequestException($"Este produto já foi indicado para empresa: {indication.Document}");
                }
            }

            try
            {
                _repository.Create(indication);
                return indication;
            }
            catch (Exception e) {
                throw new Exception($"Algo deu errado: {e}, tente mais tarde ou entre em contato!");
            }
        }

        public IndicationPaginated GetIndications(int id, string text, int page, int limit) 
        {
            List<Indication> indications = new List<Indication>();
            IndicationPaginated indicationPaginated = new IndicationPaginated();
            try
            {
                
                if (text.Equals("POLL") || text.Equals("CREATED"))
                {
                    indications = _repository.GetIndications(id, page, limit);
                }
                else 
                {
                    indications = _repository.search(id, text, page, limit);
                }
                 
                int total = _repository.countProducts();
                indicationPaginated.indications = indications;
                indicationPaginated.total = total;

            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }

            if (indications.Count < 1)
            {
                throw new NotFoundException("Nenhuma indicação encontrada, faça sua primeira :)");
            }

            return indicationPaginated;
        }

        public Indication GetById(int id) 
        {
            IndicationUtil Util = new IndicationService();
            if (id == 0) 
            {
                throw new BadRequestException("ID deve ser diferente de 0");
            }

            Indication indication = _repository.GetById(id);

            if (Util.ObjectIsNull(indication)) 
            {
                throw new NotFoundException("Nenhuma indicação encontrada!");
            }

            return indication;
        }

        public List<Indication> GetIndicationsByProduct(List<int> companies, String initialDate, String finishDate)
        {
            DateTime initialDateTime = new DateTime();

            DateTime finishDateTime = new DateTime();

            if (!String.IsNullOrEmpty(initialDate)) 
            {
                initialDateTime = DateTime.Parse(initialDate);
            }

            if (!String.IsNullOrEmpty(finishDate))
            {
                finishDateTime = DateTime.Parse(finishDate + " 23:59:59");
            }

            if (companies.Count < 1) 
            {
                throw new BadRequestException("Algo deu errado, atualize a página ou entre em contato com suporte!");
            }

            List<Product> products = _productRepository.getProductsByCompaniesId(companies);

            if (products.Count < 1) 
            {
                throw new NotFoundException("Nenhum produto para ser indicado");
            }

            List<int> productIds = new List<int>();

            foreach (Product p in products) 
            {
                productIds.Add(p.Id);
            }



            List<Indication> indications = _repository.GetByProductIds(productIds, initialDateTime, finishDateTime);

            if (indications.Count() < 1)
            {
                throw new NotFoundException("Nenhuma indicação encontrada!");
            }

            return indications;
        }

        public Indication ChangeIndicationStatus(int indicationId, IndicationStatus newStatus) 
        {
            if (indicationId == 0) 
            {
                throw new BadRequestException("Algo deu errado, atualize a página ou entre em contato com suporte!");
            }

            Indication indication = new Indication();

            try 
            {
                indication = _repository.GetById(indicationId);
            } catch (Exception e) 
            {
                throw new Exception($"Algo deu errado: {e}");
            }

            if (indication == null) 
            {
                throw new NotFoundException("Nenhuma indicação encontrada!");
            }

            if (indication.Status.Equals(newStatus)) 
            {
                throw new BadRequestException("Esta indicação já está nessa etapa!");
            }

            indication.Status = newStatus;

            try 
            {
                _repository.Update(indication);
                return indication;
            } catch (Exception e) 
            {
                throw new Exception($"Algo deu errado: {e}");
            }
        }
    }
}
