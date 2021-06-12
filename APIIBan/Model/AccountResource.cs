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
        }

        public string AccountID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdraw { get; set; }
        public decimal Transfer { get; set; }
        public string DestinationAcoountID { get; set; }

        public ResponseMessage Response { get; set; }

        public class ResponseMessage
        {
            public string StatusCode { get { return string.IsNullOrEmpty(ErrorMessage) ? "0000" : "400"; } set { } }
            public string ErrorMessage { get; set; }
            public bool IsError { get { return string.IsNullOrEmpty(ErrorMessage) ? false : true; } set { } }
        }
    }
}
