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
        Task NewAccount(AccountResource resource);
        Task Deposit(AccountResource resource);
        Task Transfer(AccountResource resource);
        bool IsSystemAuthen(string key);
        Task<List<Account>> GetAccounts();
    }
}
