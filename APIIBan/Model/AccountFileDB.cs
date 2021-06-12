using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Model
{
    public class AccountFileDB
    {
        private Decimal DepositFeeChargedPercent = (decimal)0.01;
        public AccountFileDB()
        {
            this.TransactionHistory = this.TransactionHistory ?? new List<TransactionFileDB>();
        }
        public string AccountID { get; set; }
        public Decimal Balance { get; set; }
        public List<TransactionFileDB> TransactionHistory { get; set; }

        public override bool Equals(Object obj)
        {
            try
            {
                if (obj != null && ((AccountFileDB)obj).AccountID.Equals(this.AccountID))
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { return false; }
        }

        public void Deposit(decimal amount, string  accountID, bool isFee)
        {
            var fee = amount * DepositFeeChargedPercent;
            var deposit = amount - (isFee ? fee:0);
            this.Balance += deposit;
            TransactionFileDB nt = new TransactionFileDB
            {
                Type = "D",
                TransDate = DateTime.Now,
                Amount = deposit,
                Fee = fee,
                TransferAccountID = accountID
            };
            TransactionHistory.Add(nt);
        }

        public void Withdraw(decimal amount, string accountID)
        {
            this.Balance -= amount;
            TransactionFileDB nt = new TransactionFileDB
            {
                Type = "W",
                TransDate = DateTime.Now,
                Amount = amount,
                Fee = 0,
                TransferAccountID = accountID
            };
            TransactionHistory.Add(nt);
        }

    }

    public class TransactionFileDB
    {
        public DateTime TransDate { get; set; }
        public string Type { get; set; } //W: WithDraw, D: Deposit
        public Decimal Amount { get; set; }
        public Decimal Fee { get; set; }

        public string TransferAccountID { get; set; }

    }
}
