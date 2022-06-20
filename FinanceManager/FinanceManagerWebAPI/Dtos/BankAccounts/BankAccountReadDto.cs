using System;
using FinanceManagerData.Models;

namespace FinanceManagerWebAPI.Dtos.BankAccounts
{
    public record BankAccountReadDto(string Id, double Balance);
}
