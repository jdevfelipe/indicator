using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Enums;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class IndicationController : Controller
    {
        private readonly IndicationService _service;

        public IndicationController(IndicationService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("add-new")]
        [Authorize(Roles = "INDICATOR, MANAGER")]
        public Indication InsertNew([FromBody] Indication indication)
        {
            return _service.InsertNew(indication);
        }

        [HttpGet]
        [Route("list-indications/{id}")]
        [Authorize(Roles = "INDICATOR, MANAGER")]
        public IndicationPaginated GetIndications(int id, [FromHeader] string text, [FromHeader] int page, [FromHeader] int limit)
        {
            return _service.GetIndications(id, text, page, limit);
        }

        [HttpGet]
        [Route("list-indications-by-product")]
        [Authorize(Roles = "USER, MANAGER")]
        public List<Indication> GetIndicationsByProduct([FromHeader] List<int> companies, [FromHeader] String initialDate, [FromHeader] String finishDate)
        {
            return _service.GetIndicationsByProduct(companies, initialDate, finishDate);
        }

        [HttpGet]
        [Route("change-indication-status/{indicationId}")]
        [Authorize(Roles = "USER, MANAGER")]
        public Indication ChangeIndicationStatus([FromHeader] IndicationStatus status, int indicationId)
        {
            return _service.ChangeIndicationStatus(indicationId, status);
        }
    }
}
