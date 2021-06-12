using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Model
{
    public class Transaction
    {
        [Column("s_account_id"), Key, MaxLength(50)]
        public string AccountID { get; set; }

        [Column("i_trans_id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity) ]
        public int TransID { get; set; }

        [Column("c_type"), MaxLength(2)]
        public string Type { get; set; }
        [Column("f_amount")]
        public decimal Amount { get; set; }
        [Column("d_create"), MaxLength(1)]
        public DateTime CreateDate { get; set; }
        [Column("d_change"), MaxLength(1)]
        public DateTime ChangeDate { get; set; }
        [Column("c_status"), MaxLength(2)]
        public string Status { get; set; }
    }
}
