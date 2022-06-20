using FinanceManagerData.Models;

namespace FinanceManagerWebAPI.Dtos.Finances
{
    public record IncomeWriteDto(
        Income.IncomeType Type,
        double Amount,
        string BankAccountId);
}
