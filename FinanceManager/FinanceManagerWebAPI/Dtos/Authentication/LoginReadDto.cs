using System;

namespace FinanceManagerWebAPI.Dtos.Authentication
{
    public record LoginReadDto(
        string UserName,
        string Token,
        DateTime Expires);
}
