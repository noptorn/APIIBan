using AllnowRiderAPI.Data;
using APIIBan.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIIBan.Repositories
{
    public class AccountRepository : RepositoryBase<dynamic>, IAccountRepository
    {
        private string filePath = "./Data/Data.json";
        private readonly BanDB _context;

        public AccountRepository(BanDB context) : base(context)
        {
            this._context = context;
        }

        public async Task NewAccount(Account act, AccountResource resource)
        {
            try
            {
                using (var con = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Accounts.Add(act);
                        _context.Transactions.Add(act.TransactionsHistory[0]);
                        _context.SaveChanges();
                        await con.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await con.RollbackAsync();
                        resource.Response.ErrorMessage = string.Format("Register Save fail! : {0}", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                resource.Response.ErrorMessage = string.Format("Register Connection fail!: " + ex.Message);
            }

        }


        public async Task<Account> GetAccount(string accountID)
        {
            return  await this._context.Accounts.Where(x => x.AccountID.Equals(accountID)).AsNoTracking().FirstOrDefaultAsync();
        }

        public AccountFileDB GetAccountFile(string accountID)
        {
            AccountFileDB nact = new AccountFileDB()
            {
                AccountID = accountID
            };

            List<AccountFileDB> acts = this.LoadAccountDB();
            if (acts.Contains(nact))
            {
                nact = acts[acts.IndexOf(nact)];
            }

            return nact;
        }

        public void SaveAccountDB(List<AccountFileDB> accounts)
        {
            string val = JsonSerializer.Serialize(accounts);
            File.WriteAllText(filePath, val, Encoding.UTF8);
        }

        private List<AccountFileDB> LoadAccountDB()
        {
            List<AccountFileDB> accts = new List<AccountFileDB>();
            try
            {
                string val = File.ReadAllText(filePath);
                accts = JsonSerializer.Deserialize<List<AccountFileDB>>(val);

            }
            catch (Exception ex)
            {

            }

            return accts;
        }

        public async Task Deposit(Account act, AccountResource resource)
        {
            try
            {
                using (var con = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        var dbAct = _context.Accounts.Where(x => x.AccountID.Equals(act.AccountID)).FirstOrDefault();
                        dbAct.Balance = act.Balance;
                        dbAct.ChangeDate = DateTime.Now;
                        Transaction tran = new Transaction()
                        {
                            AccountID = act.AccountID,
                            Amount = resource.Deposit,
                            Status = "Y",
                            Type = "D",
                            CreateDate = DateTime.Now,
                            ChangeDate = DateTime.Now
                        };
                        _context.Transactions.Add(tran);
                        _context.SaveChanges();
                        await con.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await con.RollbackAsync();
                        resource.Response.ErrorMessage = string.Format("Deposit Save fail! : {0}", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                resource.Response.ErrorMessage = string.Format("Deposit Connection fail!: " + ex.Message);
            }
        }

        public async Task Transfer(Account account, Account desAccount, AccountResource resource)
        {
            try
            {
                using (var con = this._context.Database.BeginTransaction())
                {
                    try
                    {
                        await this.SetAccount(account, -1 * resource.Transfer, "W");
                        await this.SetAccount(desAccount, resource.Transfer, "D");

                        _context.SaveChanges();
                        await con.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await con.RollbackAsync();
                        resource.Response.ErrorMessage = string.Format("Deposit Save fail! : {0}", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                resource.Response.ErrorMessage = string.Format("Deposit Connection fail!: " + ex.Message);
            }
        }

        private async Task SetAccount(Account act, decimal amount, string type)
        {
            var dbSrcAct = _context.Accounts.Where(x => x.AccountID.Equals(act.AccountID)).FirstOrDefault();
            dbSrcAct.Balance = act.Balance;
            dbSrcAct.ChangeDate = DateTime.Now;
            Transaction srcTran = new Transaction()
            {
                AccountID = act.AccountID,
                Amount = amount,
                Status = "Y",
                Type = type,
                CreateDate = DateTime.Now,
                ChangeDate = DateTime.Now
            };
            _context.Transactions.Add(srcTran);
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await this._context.Accounts.AsNoTracking().ToListAsync<Account>();
        }
    }
}
