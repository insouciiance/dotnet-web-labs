using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinanceManagerData.Models;

namespace FinanceManagerData.Contexts
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public override DbSet<ApplicationUser> Users { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Income> Incomes { get; set; }

        public DbSet<Expenditure> Expenditures { get; set; }
        
        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<BankAccount>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.BankAccounts)
                .HasForeignKey(a => a.OwnerId);

            builder
                .Entity<Transaction>()
                .HasOne(t => t.SenderAccount)
                .WithMany(a => a.SenderTransactions)
                .HasForeignKey(t => t.SenderAccountId);

            builder
                .Entity<Transaction>()
                .HasOne(t => t.ReceiverAccount)
                .WithMany(a => a.ReceiverTransactions)
                .HasForeignKey(t => t.ReceiverAccountId);

            builder
                .Entity<Expenditure>()
                .HasOne(e => e.BankAccount)
                .WithMany(a => a.Expenditures)
                .HasForeignKey(e => e.BankAccountId);

            builder
                .Entity<Income>()
                .HasOne(i => i.BankAccount)
                .WithMany(a => a.Incomes)
                .HasForeignKey(i => i.BankAccountId);

            base.OnModelCreating(builder);
        }
    }
}
