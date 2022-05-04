using System.Threading.Tasks;
using FinanceManagerData.Models;

namespace FinanceManagerData.Repositories
{
    public interface IAppRepository
    {
        ApplicationUser GetUserById(string id);

        BankAccount GetBankAccountById(string id);

        Expenditure GetExpenditureById(string id);

        Income GetIncomeById(string id);

        Transaction GetTransactionById(string id);

        void AddUser(ApplicationUser user);

        void AddBankAccount(BankAccount account);

        void AddExpendutire(Expenditure expenditure);

        void AddIncome(Income income);

        void AddTransaction(Transaction transaction);

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}