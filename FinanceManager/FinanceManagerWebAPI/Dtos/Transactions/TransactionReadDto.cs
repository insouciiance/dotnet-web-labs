using System;

namespace FinanceManagerWebAPI.Dtos.Transactions
{
    public record TransactionReadDto(
        double Amount,
        string SenderAccountId,
        string ReceiverAccountId,
        DateTime Date);
}
