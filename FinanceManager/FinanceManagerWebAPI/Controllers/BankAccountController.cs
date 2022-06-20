using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceManagerData.Models;
using FinanceManagerData.Repositories;
using FinanceManagerWebAPI.Dtos.BankAccounts;

namespace FinanceManagerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private IHttpContextAccessor _contextAccessor;

        private IAppRepository _repository;

        public BankAccountController(
            IHttpContextAccessor contextAccessor,
            IAppRepository repository)
        {
            _contextAccessor = contextAccessor;
            _repository = repository;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            string userId = _contextAccessor.HttpContext!.User.Claims.First().Value;

            BankAccount newAccount = new()
            {
                Id = Guid.NewGuid().ToString(),
                Balance = 0,
                OwnerId = userId
            };

            _repository.AddBankAccount(newAccount);

            await _repository.SaveChangesAsync();

            return Ok(new BankAccountReadDto(
                newAccount.Id,
                newAccount.Balance));
        }
    }
}
