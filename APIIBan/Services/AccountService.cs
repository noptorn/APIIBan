using APIIBan.Model;
using APIIBan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _iAccountRepo;
        public AccountService(IAccountRepository iAccountRepo)
        {
            this._iAccountRepo = iAccountRepo;
        }

        public Account NewAccount(Account account)
        {
            return this._iAccountRepo.NewAccount(account);
        }

        public Account Deposit(Account account, decimal deposit)
        {
            return this._iAccountRepo.Deposit(account, deposit, true);
        }

        public void Transfer(AccountResource resource)
        {
            Account srcAccount = this._iAccountRepo.GetAccount(resource.Account.AccountID);
            Account toAccount = this._iAccountRepo.GetAccount(resource.DestinationAcoountID);

            if(srcAccount.Balance < resource.Withdraw)
            {
                resource.Response.IsError = true;
                resource.Response.ErrorMessage = "Money not enough.";
            } else
            {
                this._iAccountRepo.Transfer(srcAccount, resource.Withdraw, toAccount);
            }
        }

        public List<Account> GetAccounts()
        {
            return this._iAccountRepo.GetAccounts();
        }

    }
}
