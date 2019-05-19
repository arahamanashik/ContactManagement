using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManagement.Core.Dtos;
using ContactManagement.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        //private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider
        public UserInfoController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
            //_authenticationSchemeProvider = authenticationSchemeProvider;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserInfoDto userParam)
        {
            await _userInfoRepository.Register(userParam);
            await _userInfoRepository.CommitAsync();
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("thirdpartyregister")]
        public async Task<IActionResult> Thirdpartyregister()
        {
            UserInfoDto userParam = new UserInfoDto();
            userParam.Email = User.Identity.Name;

            await _userInfoRepository.Register(userParam);
            await _userInfoRepository.CommitAsync();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserInfoDto userParam)
        {
            var user = _userInfoRepository.Authenticate(userParam.Email, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null) =>
            Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);

    }
}