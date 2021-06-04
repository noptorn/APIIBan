using APIIBan.Model;
using APIIBan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Threading.Tasks;

namespace APIIBan.Controllers
{
    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private IAccountService _iactService;
        private INodeServices _nodeService;

        [System.Obsolete]
        public BankController(IAccountService accountService, INodeServices nodeService)
        {
            _iactService = accountService;
            _nodeService = nodeService;
        }

        public async Task<string> GetAccountID()
        {
            string id = string.Empty;
            try
            {
                id = await _nodeService.InvokeAsync<string>("./js/Call.js");
            }
            catch (Exception ex)
            {
                //Exception
            }

            return id;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountResource resource)
        {

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(resource.Account.AccountID))
                {
                    resource.Account.AccountID = await this.GetAccountID();
                }

                resource.Account = this._iactService.NewAccount(resource.Account);
                resource.Response.StatusCode = "0000";
            }
            else
            {
                resource.Response.IsError = true;
                resource.Response.StatusCode = "400";
                resource.Response.ErrorMessage = "Model state is unvalid";
            }

            return StatusCode(resource.Response.IsError ? 400 : 200, resource);
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] AccountResource resource)
        {
            if (ModelState.IsValid)
            {
                resource.Account = this._iactService.Deposit(resource.Account, resource.Deposit);
                resource.Response.StatusCode = "0000";
            }
            else
            {
                resource.Response.IsError = true;
                resource.Response.StatusCode = "400";
                resource.Response.ErrorMessage = "Model state is unvalid";
            }

            return StatusCode(resource.Response.IsError ? 400 : 200, resource);
        }


        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] AccountResource resource)
        {
            if (ModelState.IsValid)
            {
                this._iactService.Transfer(resource);
                resource.Response.StatusCode = !resource.Response.IsError ? "0000" : "9999";
            }
            else
            {
                resource.Response.IsError = true;
                resource.Response.StatusCode = "400";
                resource.Response.ErrorMessage = "Model state is unvalid";
            }

            return StatusCode(resource.Response.IsError ? 400 : 200, resource);
        }

        [HttpGet("accounts")]
        public IActionResult GetAccount()
        {
            var acts = this._iactService.GetAccounts();

            return StatusCode(200, acts);
        }
    }
}
