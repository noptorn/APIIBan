using APIIBan.Model;
using APIIBan.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _iAccountRepo;
        private decimal _depositFeeChargedPercent = (decimal)0.01;
       
        public IConfiguration Configuration { get; }
        public AccountService(IAccountRepository iAccountRepo, IConfiguration configuration)
        {
            this._iAccountRepo = iAccountRepo;
            Configuration = configuration;
        }

        public bool IsSystemAuthen(string parKey)
        {
            string key = Configuration["System:Key"];
            return key.Equals(parKey);
        }

        public async Task NewAccount(AccountResource resource)
        {
            Account act = new Account() {
                AccountID = resource.AccountID,
                Balance = this.Deposit(resource.Deposit, true),
                FirstName = resource.FirstName,
                LastName = resource.LastName,
                CreateDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Status = "Y",
                TransactionsHistory = new List<Transaction>()
            };
            act.TransactionsHistory.Add(new Transaction()
            {
                AccountID = act.AccountID,
                CreateDate = DateTime.Now,
                ChangeDate = DateTime.Now,
                Amount = act.Balance,
                Status = "Y",
                Type = "D"
            });

            this._iAccountRepo.NewAccount(act, resource);
        }

        public decimal Deposit(decimal amount, bool isFee)
        {
            var fee = amount * _depositFeeChargedPercent;
            var deposit = amount - (isFee ? fee : 0);
          
            return deposit;
        }

        public async Task Deposit(AccountResource resource)
        {
            Account act = await this._iAccountRepo.GetAccount(resource.AccountID);
            act.Balance += this.Deposit(resource.Deposit, true);
            await this._iAccountRepo.Deposit(act, resource);
        }

        public async Task Transfer(AccountResource resource)
        {
            Account srcAccount = await this._iAccountRepo.GetAccount(resource.AccountID);
            Account desAccount = await this._iAccountRepo.GetAccount(resource.DestinationAcoountID);

            if (srcAccount.Balance < resource.Transfer)
            {
                resource.Response.ErrorMessage = "Money not enough.";
            }
            else
            {
                srcAccount.Balance -= resource.Transfer;
                desAccount.Balance += resource.Transfer;

                await this._iAccountRepo.Transfer(srcAccount, desAccount, resource);
            }
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await this._iAccountRepo.GetAccounts();
        }

    }
}
