using APIIBan.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIIBan.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private string filePath = "./Data/Data.json";


        public Account NewAccount(Account account)
        {
            Account nact = account;

            List<Account> acts = this.LoadAccountDB();
            if (acts.Contains(nact))
            {
                nact = acts[acts.IndexOf(nact)];
            }
            else
            {
                acts.Add(nact);
            }
            this.SaveAccountDB(acts);

            return nact;
        }

        public Account GetAccount(string accountID)
        {
            Account nact = new Account()
            {
                AccountID = accountID
            };

            List<Account> acts = this.LoadAccountDB();
            if (acts.Contains(nact))
            {
                nact = acts[acts.IndexOf(nact)];
            }

            return nact;
        }

        public void SaveAccountDB(List<Account> accounts)
        {
            string val = JsonSerializer.Serialize(accounts);
            File.WriteAllText(filePath, val, Encoding.UTF8);
        }

        private List<Account> LoadAccountDB()
        {
            List<Account> accts = new List<Account>();
            try
            {
                string val = File.ReadAllText(filePath);
                accts = JsonSerializer.Deserialize<List<Account>>(val);

            }
            catch (Exception ex)
            {

            }

            return accts;
        }

        public Account Deposit(Account account, decimal deposit, bool isFee)
        {
            Account nact = account;            
            List<Account> acts = this.LoadAccountDB();
            if (acts.Contains(nact))
            {
                nact = acts[acts.IndexOf(nact)];
            }
            else
            {
                acts.Add(nact);
            }
            nact.Deposit(deposit, string.Empty, isFee);
            
            this.SaveAccountDB(acts);

            return nact;
        }

        public Account Transfer(Account account, decimal withdraw, Account toAccount)
        {
            List<Account> acts = this.LoadAccountDB();

            Account sAct = acts[acts.IndexOf(account)];
            Account dAct = acts[acts.IndexOf(toAccount)];

            sAct.Withdraw(withdraw, toAccount.AccountID);
            dAct.Deposit(withdraw, sAct.AccountID, false);

            this.SaveAccountDB(acts);

            return sAct;
        }

        public List<Account> GetAccounts()
        {
            return this.LoadAccountDB();
        }
    }
}
