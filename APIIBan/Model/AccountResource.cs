using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIBan.Model
{
    public class AccountResource
    {
        public AccountResource()
        {
            this.Response = this.Response ?? new ResponseMessage();
            this.Account = this.Account ?? new Account();
        }

        public Account Account {get;set;}

        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }
        public string DestinationAcoountID { get; set; }

        public ResponseMessage Response { get; set; }

        public class ResponseMessage
        {
            public string StatusCode { get; set; }
            public string ErrorMessage { get; set; }
            public bool IsError { get; set; }
        }
    }
}
