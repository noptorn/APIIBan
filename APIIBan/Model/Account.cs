using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Model
{
    public class Account
    {
        [Column("s_account_id"), Key, MaxLength(50)]
        public string AccountID { get; set; }
        [Column("s_first_name"), MaxLength(50)]
        public string FirstName { get; set; }
        [Column("s_last_name"), MaxLength(50)]
        public string LastName { get; set; }
        [Column("c_status"), MaxLength(1)]  
        public string Status { get; set; }
        [Column("d_create"), MaxLength(1)]
        public DateTime CreateDate { get; set; }
        [Column("d_change"), MaxLength(1)]
        public DateTime ChangeDate { get; set; }

        [Column("f_balance")]
        public decimal Balance { get; set; }

        public List<Transaction> TransactionsHistory { get; set; }
    }
}
