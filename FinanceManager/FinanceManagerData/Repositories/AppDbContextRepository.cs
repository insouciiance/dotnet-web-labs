using System.Threading.Tasks;
using FinanceManagerData.Contexts;
using FinanceManagerData.Models;

namespace FinanceManagerData.Repositories
{
    public class AppDbContextRepository : IAppRepository
    {
        private AppDbContext _dbContext;

        public AppDbContextRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddBankAccount(BankAccount account)
        {
            _dbContext.BankAccounts.Add(account);
        }

        public void AddExpendutire(Expenditure expenditure)
        {
            _dbContext.Expenditures.Add(expenditure);
        }

        public void AddIncome(Income income)
        {
            _dbContext.Incomes.Add(income);
        }

        public void AddTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }

        public void AddUser(ApplicationUser user)
        {
            _dbContext.Users.Add(user);
        }

        public BankAccount GetBankAccountById(string id)
        {
            return _dbContext.BankAccounts.Find(id);
        }

        public Expenditure GetExpenditureById(string id)
        {
            return _dbContext.Expenditures.Find(id);
        }

        public Income GetIncomeById(string id)
        {
            return _dbContext.Incomes.Find(id);
        }

        public Transaction GetTransactionById(string id)
        {
            return _dbContext.Transactions.Find(id);
        }

        public ApplicationUser GetUserById(string id)
        {
            return _dbContext.Users.Find(id);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
