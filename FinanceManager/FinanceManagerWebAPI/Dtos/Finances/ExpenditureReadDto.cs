using System;
using FinanceManagerData.Models;

namespace FinanceManagerWebAPI.Dtos.Finances
{
    public record ExpenditureReadDto(
        Expenditure.ExpenditureKind Kind,
        double Amount,
        string BankAccountId,
        DateTime Date);
}
