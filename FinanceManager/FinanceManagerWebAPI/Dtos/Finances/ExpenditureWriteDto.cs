using FinanceManagerData.Models;

namespace FinanceManagerWebAPI.Dtos.Finances
{
    public record ExpenditureWriteDto(
        Expenditure.ExpenditureKind Kind,
        double Amount,
        string BankAccountId);
}
