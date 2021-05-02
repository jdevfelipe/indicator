using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace indicator_api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _service;

        public CompanyController(CompanyService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("add-new")]
        [Authorize(Roles = "ANALYST")]
        public Company InsertNew([FromBody] CompanyRegister company)
        {
            return _service.InsertNew(company);
        }

        [HttpGet]
        [Route("get-companies")]
        [Authorize(Roles = "ANALYST")]
        public CompanyPaginatedDTO GetCompanies([FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetCompanies(page, limit);
        }


        [HttpGet]
        [Route("get-companies-by-id")]
        [Authorize(Roles = "MANAGER")]
        public List<CompanyRegister> GetCompaniesById([FromHeader] List<int> companiesIds)
        {
            return _service.GetCompaniesById(companiesIds);
        }

        [HttpPost]
        [Route("update")]
        [Authorize(Roles = "ANALYST, MANAGER")]
        public UserRegisterDTO UpdateUser([FromBody] User user)
        {
            return _service.UpdateUser(user);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "ANALYST")]
        public UserRegisterDTO DeleteUser([FromHeader] int id)
        {
            return _service.DeleteUser(id);
        }

        [HttpPost]
        [Route("update/company")]
        [Authorize(Roles = "ANALYST, MANAGER")]
        public Company UpdateCompany([FromBody] Company company)
        {
            return _service.UpdateCompany(company);
        }


        [HttpGet]
        [Route("forgot-pass")]
        [AllowAnonymous]
        public Task<bool> ForgotPass([FromHeader] String document)
        {
            return _service.ForgotPassAsync(document);
        }

    }
}
