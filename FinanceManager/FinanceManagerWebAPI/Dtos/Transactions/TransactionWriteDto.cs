namespace FinanceManagerWebAPI.Dtos.Transactions
{
    public record TransactionWriteDto(
        double Amount,
        string SenderAccountId,
        string ReceiverAccountId);
}
