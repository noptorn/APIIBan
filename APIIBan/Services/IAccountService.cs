using APIIBan.Model;
using APIIBan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Services
{
    public interface IAccountService
    {
        Account NewAccount(Account resource);
        Account Deposit(Account resource, decimal deposit);
        void Transfer(AccountResource resource);
        List<Account> GetAccounts();
    }
}
