using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinanceManagerData.Models;
using FinanceManagerData.Repositories;
using FinanceManagerServices.Validation;
using FinanceManagerWebAPI.Dtos.Transactions;

namespace FinanceManagerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private IHttpContextAccessor _contextAccessor;

        private IAppRepository _repository;

        public TransactionsController(
            IHttpContextAccessor contextAccessor,
            IAppRepository repository)
        {
            _contextAccessor = contextAccessor;
            _repository = repository;
        }

        [Authorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody]TransactionWriteDto dto)
        {
            if (dto is null)
            {
                return BadRequest(ErrorMessages.UnexpectedError);
            }

            string userId = _contextAccessor.HttpContext.User.Claims.First().Value;

            BankAccount senderAccount = _repository.GetBankAccountById(dto.SenderAccountId);
            BankAccount receiverAccount = _repository.GetBankAccountById(dto.ReceiverAccountId);

            if (senderAccount.OwnerId != userId)
            {
                return Forbid();
            }

            if (dto.Amount <= 0)
            {
                return BadRequest(ErrorMessages.BadTransactionAmount);
            }

            if (senderAccount.Balance - dto.Amount < 0)
            {
                return BadRequest(ErrorMessages.InsufficientBalance);
            }

            Transaction newTransaction = new()
            {
                Id = Guid.NewGuid().ToString(),
                Amount = dto.Amount,
                Date = DateTime.Now,
                SenderAccountId = dto.SenderAccountId,
                ReceiverAccountId = dto.ReceiverAccountId,
            };

            _repository.AddTransaction(newTransaction);

            senderAccount.Balance -= dto.Amount;
            receiverAccount.Balance += dto.Amount;

            await _repository.SaveChangesAsync();

            return Ok(new TransactionReadDto(
                newTransaction.Amount,
                newTransaction.SenderAccountId,
                newTransaction.ReceiverAccountId,
                newTransaction.Date));
        }
    }
}