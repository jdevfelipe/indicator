using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("add-new")]
        [Authorize(Roles = "MANAGER")]
        public Product InsertNew([FromBody] ProductDTO product)
        {
            return _service.AddNew(product);
        }

        [HttpGet]
        [Route("get-products")]
        [Authorize(Roles = "MANAGER, INDICATOR")]
        public ProductsPaginatedDTO getProducts([FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetProducts(page, limit);
        }

        [HttpGet]
        [Route("get-product/{productId}")]
        [Authorize(Roles = "MANAGER, INDICATOR")]
        public Product getProduct(int productId)
        {
            return _service.GetProduct(productId);
        }

        [HttpGet]
        [Route("get-products-by-company/")]
        [Authorize(Roles = "MANAGER")]
        public ProductPaginated getProductsByCompany([FromHeader] List<int> companyIds, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetProductsByCompaniesId(companyIds, page, limit);
        }

        [HttpGet]
        [Route("search")]
        [Authorize(Roles = "MANAGER, INDICATOR")]
        public ProductsPaginatedDTO GetProductsSearch([FromHeader] String searchTerm, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetProductsSearch(searchTerm, page, limit);
        }
    }
}
