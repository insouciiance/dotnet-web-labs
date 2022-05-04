using System;
using FinanceManagerData.Models;

namespace FinanceManagerWebAPI.Dtos.Finances
{
    public record IncomeReadDto(
        Income.IncomeType Type,
        double Amount,
        string BankAccountId,
        DateTime Date);
}
