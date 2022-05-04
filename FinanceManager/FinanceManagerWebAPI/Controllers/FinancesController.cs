using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceManagerData.Models;
using FinanceManagerData.Repositories;
using FinanceManagerServices.Validation;
using FinanceManagerWebAPI.Dtos.Finances;

namespace FinanceManagerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancesController : ControllerBase
    {
        private IHttpContextAccessor _contextAccessor;

        private IAppRepository _repository;

        public FinancesController(
            IHttpContextAccessor contextAccessor,
            IAppRepository repository)
        {
            _contextAccessor = contextAccessor;
            _repository = repository;
        }

        [Authorize]
        [HttpPost]
        [Route("gain")]
        public async Task<IActionResult> Gain([FromBody]IncomeWriteDto dto)
        {
            string userId = _contextAccessor.HttpContext.User.Claims.First().Value;

            BankAccount incomeAccount = _repository.GetBankAccountById(dto.BankAccountId);

            if (incomeAccount is null)
            {
                return BadRequest(ErrorMessages.UnexpectedError);
            }

            if (incomeAccount.OwnerId != userId)
            {
                return Forbid();
            }

            if (dto.Amount <= 0)
            {
                return BadRequest(ErrorMessages.BadIncomeAmount);
            }

            Income newIncome = new()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = dto.Amount,
                BankAccountId = incomeAccount.Id,
                Date = DateTime.Now,
                Type = dto.Type
            };

            incomeAccount.Balance += newIncome.Amount;

            _repository.AddIncome(newIncome);

            await _repository.SaveChangesAsync();

            return Ok(new IncomeReadDto(
                newIncome.Type,
                newIncome.Amount,
                newIncome.BankAccountId,
                newIncome.Date
            ));
        }

        [Authorize]
        [HttpPost]
        [Route("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody]ExpenditureWriteDto dto)
        {
            string userId = _contextAccessor.HttpContext.User.Claims.First().Value;

            BankAccount expenditureAccount = _repository.GetBankAccountById(dto.BankAccountId);

            if (expenditureAccount is null)
            {
                return BadRequest(ErrorMessages.UnexpectedError);
            }

            if (expenditureAccount.OwnerId != userId)
            {
                return Forbid();
            }

            if (dto.Amount <= 0)
            {
                return BadRequest(ErrorMessages.BadIncomeAmount);
            }

            if (expenditureAccount.Balance - dto.Amount < 0)
            {
                return BadRequest(ErrorMessages.InsufficientBalance);
            }

            Expenditure newExpenditure = new()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = dto.Amount,
                BankAccountId = expenditureAccount.Id,
                Date = DateTime.Now,
                Kind = dto.Kind
            };

            expenditureAccount.Balance -= dto.Amount;

            _repository.AddExpendutire(newExpenditure);

            await _repository.SaveChangesAsync();

            return Ok(new ExpenditureReadDto(
                newExpenditure.Kind,
                newExpenditure.Amount,
                newExpenditure.BankAccountId,
                newExpenditure.Date
            ));

        }
    }
}