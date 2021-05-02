using System;
using indicator_api.Entities;
using indicator_api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: /
        [HttpGet]
        public string Get()
        {
            return "I'm Running";
        }

        // GET /5
        [HttpGet]
        [Authorize(Roles = "ANALYST")]
        [Route("company/{id}")]
        public string TestAuthCompany(int id)
        {
            return "value";
        }

        // GET /5
        [HttpGet]
        [Authorize(Roles = "INDICATOR")]
        [Route("indicator/{id}")]
        public string TestAuthIndicator(int id)
        {
            return "value";
        }

        // POST /
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return value;
        }

        // PUT /5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE /5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
