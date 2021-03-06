using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagerData.Models
{
    public class Income
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public IncomeType Type { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public enum IncomeType
        {
            Salary,
            Rent,
            Scholarship,
            Other
        }
    }
}
