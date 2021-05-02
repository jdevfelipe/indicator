using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using indicator_api.DTOs;
using indicator_api.Entities;
using indicator_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace indicator_api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IndicatorService _indicatorService;

        public AuthController(UserService userService, IndicatorService indicatorService)
        {
            _userService = userService;
            _indicatorService = indicatorService;
        }


        // POST v1/<UserController>
        [HttpPost]
        [Route("company/login")]
        [AllowAnonymous]
        public IActionResult LoginUserCompany([FromBody] User user)
        {
            UserLoginDTO userLogged = _userService.Login(user.CPF, user.Password);
            return Ok(userLogged);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("company/register")]
        [Authorize(Roles = "ANALYST")]
        public IActionResult RegisterUserCompany([FromBody] User user)
        {
            UserLoginDTO userLogged = _userService.RegisterNew(user);
            return Ok(userLogged);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("indicator/login")]
        [AllowAnonymous]
        public IActionResult LoginIndicator([FromBody] Indicator indicator)
        {
            IndicatorLoginDTO indicatorLogged = _indicatorService.Login(indicator.CPF, indicator.Password);
            return Ok(indicatorLogged);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("indicator/register")]
        [AllowAnonymous]
        public IActionResult RegisterIndicatorAsync([FromBody] Indicator indicator)
        {
            IndicatorLoginDTO indicatorLogged = _indicatorService.RegisterNewAsync(indicator);
            return Ok(indicatorLogged);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("indicator/verify-code/{code}")]
        [AllowAnonymous]
        public IActionResult VerifyIfCodeIsOk(long code, [FromBody] Indicator indicator)
        {
            IndicatorLoginDTO indicatorLogged = _indicatorService.VerifyIfCodeIsOk(indicator, code);
            return Ok(indicatorLogged);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("indicator/generate-new-code")]
        [AllowAnonymous]
        public IActionResult GenerateNewCode([FromBody] Indicator indicator)
        {
            Task<bool> res = _indicatorService.GenerateNewCodeAsync(indicator);
            return Ok(res);
        }

        // POST v1/<UserController>
        [HttpPost]
        [Route("indicator/validate-token")]
        [AllowAnonymous]
        public IActionResult TokenIsValid([FromBody] IndicatorLoginDTO indicator)
        {
            bool res = TokenService.ValidateCurrentToken(indicator.Token);
            return Ok(res);
        }
    }
}
