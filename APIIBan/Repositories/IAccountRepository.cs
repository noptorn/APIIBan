using APIIBan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Repositories
{
    public interface IAccountRepository
    {
       // void SaveAccountDB(List<AccountFileDB> accounts);
        
        Task NewAccount(Account account, AccountResource resource);
        Task Deposit(Account account, AccountResource resource);
        Task<Account> GetAccount(string accountID);
        Task Transfer(Account account, Account toAccount, AccountResource resource);
        Task<List<Account>> GetAccounts();
    }
}
