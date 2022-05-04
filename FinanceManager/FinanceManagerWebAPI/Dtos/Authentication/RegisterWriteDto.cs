namespace FinanceManagerWebAPI.Dtos.Authentication
{
    public record RegisterWriteDto(
        string UserName,
        string Email,
        string Password,
        string PasswordConfirm);
}
