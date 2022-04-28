using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FinanceManagerData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<BankAccount> BankAccounts { get; set; }
    }
}