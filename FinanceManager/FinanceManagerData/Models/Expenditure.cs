using System.ComponentModel.DataAnnotations;

namespace FinanceManagerData.Models
{
    public class Expenditure
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public ExpenditureKind Kind { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string BankAccountId { get; set; }

        public BankAccount BankAccount { get; set; }

        public enum ExpenditureKind
        {
            Utilities,
            Food,
            Entertainment,
            Other
        }
    }
}