using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagerData.Models
{
    public class BankAccount
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public double Balance { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        public virtual IEnumerable<Transaction> SenderTransactions { get; set; }

        public virtual IEnumerable<Transaction> ReceiverTransactions { get; set; }

        public virtual IEnumerable<Expenditure> Expenditures { get; set; }

        public virtual IEnumerable<Income> Incomes { get; set; }
    }
}
