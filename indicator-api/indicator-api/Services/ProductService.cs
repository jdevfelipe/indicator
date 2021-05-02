using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Exceptions;
using indicator_api.Repositories.Interfaces;
using indicator_api.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indicator_api.Services
{
    public class ProductService : ProductUtil
    {
        private readonly ProductRepository _repository;

        public ProductService()
        {
        }

        public ProductService(ProductRepository repository)
        {
            _repository = repository;
        }

        public Product AddNew(ProductDTO productdto)
        {
            Product product = new Product();
            product.IndicationPrice = Double.Parse(productdto.IndicationPrice);
            product.Indications = productdto.Indications;
            product.name = productdto.name;
            product.Status = productdto.Status;
            product.Company = productdto.Company;
            product.CompanyId = productdto.CompanyId;

            ProductUtil util = new ProductService();

            if (util.ObjectIsNull(product))
            {
                throw new BadRequestException("Produto não pode estar vazio");
            }

            if (util.NameIsEmty(product))
            {
                throw new BadRequestException("Nome deve ser preenchido");
            }

            if (util.PriceIsEmty(product))
            {
                throw new BadRequestException("Valor pago pela indicação deve ser prenchido");
            }

            if (util.CompanyIsEmty(product))
            {
                throw new BadRequestException("Produto não está vinculado à uma empresa");
            }

            try
            {
                _repository.Create(product);
                return product;
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }
        }

        public ProductsPaginatedDTO GetProducts(int page, int limit)
        {
            try
            {
                List<Product> products = _repository.getAllProducts(page, limit);
                List<ProductDTO> productsDTO = new List<ProductDTO>();
                int total = _repository.countProducts();

                foreach (Product p in products)
                {
                    ProductDTO productDTO = new ProductDTO();
                    productDTO.id = p.Id;
                    productDTO.name = p.name;
                    productDTO.Status = p.Status;
                    productDTO.Indications = p.Indications;
                    productDTO.Company = p.Company;
                    productDTO.IndicationPrice = p.IndicationPrice.ToString();

                    productsDTO.Add(productDTO);
                }
                ProductsPaginatedDTO productsPaginatedDTO = new ProductsPaginatedDTO();
                productsPaginatedDTO.productDTOs = productsDTO;
                productsPaginatedDTO.total = total;

                return productsPaginatedDTO;

            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }
        }

        public Product GetProduct(int productId)
        {
            ProductUtil util = new ProductService();
            if (productId == 0)
            {
                throw new BadRequestException($"Erro comn este id: {productId}");
            }

            Product product = new Product();

            try
            {
                product = _repository.getProduct(productId);
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e}");
            }

            if (util.ObjectIsNull(product))
            {
                throw new NotFoundException($"Nenhum produto encontrado com este id: {productId}");
            }

            return product;
        }

        public ProductPaginated GetProductsByCompaniesId(List<int> companiesIds, int page, int limit)
        {
            if (companiesIds.Count < 1)
            {
                throw new BadRequestException("Algo deu errado, tente recarregar a página ou entre em contato com o suporte");
            }

            List<Product> products = new List<Product>();

            try
            {
                products = _repository.getProductsByCompaniesId(companiesIds, page, limit);
            }
            catch (Exception e)
            {
                throw new Exception($"Algo deu errado: {e.GetType()}");
            }

            if (products.Count < 1) 
            {
                throw new NotFoundException("Nenhum produto cadastrado");
            }

            ProductPaginated productPaginated = new ProductPaginated();

            productPaginated.products = products;
            productPaginated.total = _repository.countProductsByCompaniesId(companiesIds);

            return productPaginated;
        }

        public ProductsPaginatedDTO GetProductsSearch(String term, int page, int limit) 
        {
            try
            {
                List<Product> products = _repository.searchProducts(term, page, limit);
                List<ProductDTO> productsDTO = new List<ProductDTO>();
                int total = _repository.countProducts();

                foreach (Product p in products) 
                {
                    ProductDTO productDTO = new ProductDTO();
                    productDTO.id = p.Id;
                    productDTO.name = p.name;
                    productDTO.Status = p.Status;
                    productDTO.Indications = p.Indications;
                    productDTO.Company = p.Company;
                    productDTO.IndicationPrice = p.IndicationPrice.ToString();

                    productsDTO.Add(productDTO);
                }
                ProductsPaginatedDTO productsPaginatedDTO = new ProductsPaginatedDTO();
                productsPaginatedDTO.productDTOs = productsDTO;
                productsPaginatedDTO.total = total;

                return productsPaginatedDTO;

            }
            catch (Exception e) 
            {
                throw new Exception($"Algo deu errado: {e}");
            }
        }
    }
}
