using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinanceManagerData.Models;
using FinanceManagerServices.Authentication;
using FinanceManagerServices.Validation;
using FinanceManagerWebAPI.Dtos.Authentication;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace FinanceManagerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager; 
        private JwtTokenHelper _jwtHelper;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            JwtTokenHelper jwtHelper
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtHelper = jwtHelper;    
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterWriteDto dto)
        {
            if (dto is null ||
                dto.UserName is null ||
                dto.Email is null ||
                dto.Password is null)
            {
                return BadRequest(ErrorMessages.BadCredentials);
            }

            if (await _userManager.FindByEmailAsync(dto.Email) is { } ||
                await _userManager.FindByNameAsync(dto.UserName) is { })
            {
                return BadRequest(ErrorMessages.UserExists);
            }

            if (dto.Password != dto.PasswordConfirm)
            {
                return BadRequest(ErrorMessages.PasswordsDontMatch);
            }

            ApplicationUser user = new()
            {
                Email = dto.Email,
                UserName = dto.UserName
            };

            IdentityResult creationResult = await _userManager.CreateAsync(user, dto.Password);

            if (!creationResult.Succeeded)
            {
                return BadRequest(ErrorMessages.UnexpectedError);
            }

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (!signInResult.Succeeded)
            {
                return BadRequest(ErrorMessages.UnexpectedError);
            }

            (string jwtToken, DateTime expires) = _jwtHelper.CreateAuthToken(user);

            return Ok(new LoginReadDto(user.UserName, jwtToken, expires));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginWriteDto dto)
        {
            if (dto is null ||
                dto.UserName is null ||
                dto.Password is null)
            {
                return BadRequest(ErrorMessages.BadCredentials);
            }

            ApplicationUser user = await _userManager.FindByNameAsync(dto.UserName);

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);

            if (!signInResult.Succeeded)
            {
                return BadRequest(ErrorMessages.BadCredentials);
            }

            (string jwtToken, DateTime expires) = _jwtHelper.CreateAuthToken(user);

            return Ok(new LoginReadDto(user.UserName, jwtToken, expires));
        }
    }
}