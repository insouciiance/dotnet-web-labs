using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FinanceManagerData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IEnumerable<BankAccount> BankAccounts { get; set; }
    }
}
