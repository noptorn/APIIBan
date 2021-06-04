using APIIBan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Repositories
{
    public interface IAccountRepository
    {
        void SaveAccountDB(List<Account> accounts);
        
        Account NewAccount(Account account);

        Account Deposit(Account account, decimal deposit, bool isFee);
        Account Transfer(Account account, decimal withdraw, Account toAccount);
        Account GetAccount(string accountID);
        List<Account> GetAccounts();
    }
}
