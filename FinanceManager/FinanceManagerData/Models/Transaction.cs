using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagerData.Models
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string SenderAccountId { get; set; }

        public string ReceiverAccountId { get; set; }

        public BankAccount SenderAccount { get; set; }

        public BankAccount ReceiverAccount { get; set; }
    }
}