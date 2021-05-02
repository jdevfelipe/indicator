using System.Collections.Generic;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace indicator_api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }



        // GET: v1/<UserController>
        [HttpGet]
        [Route("get-users")]
        [Authorize(Roles = "ANALYST")]
        public UserPaginated GetUsers([FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetUsers(page, limit);
        }

        // GET: v1/<UserController>
        [HttpGet]
        [Route("list-users-by-companies")]
        [Authorize(Roles = "MANAGER")]
        public UsersPaginated GetUsersByCompanies([FromHeader] List<int> companies, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetUsersByCompanies(companies, page, limit);
        }

        // GET: v1/<UserController>
        [HttpPost]
        [Route("insert-users")]
        [Authorize(Roles = "MANAGER")]
        public UserRegisterDTO InsertUser([FromBody] User user, [FromHeader] List<int> companiesId)
        {
            return _service.InsertNewUser(user, companiesId);
        }
    }
}
