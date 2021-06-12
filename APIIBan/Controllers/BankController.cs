using APIIBan.Model;
using APIIBan.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIIBan.Controllers
{
    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private IAccountService _iactService;
        private INodeServices _nodeService;

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

        private bool ValidateHeaderKey()
        {
            string key = this.Request.Headers["x-system-key"].ToString();
            return this._iactService.IsSystemAuthen(key);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountResource resource)
        {
            if (this.ValidateHeaderKey())
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(resource.AccountID))
                    {
                        resource.AccountID = await this.GetAccountID();
                    }

                    await this._iactService.NewAccount(resource);
                }
                else
                {
                    resource.Response.ErrorMessage = "Model state is unvalid";
                }
            }
            else
            {
                resource.Response.ErrorMessage = "Authen Fail.";
            }
            return StatusCode(resource.Response.IsError ? 400 : 200, resource);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] AccountResource resource)
        {
            if (this.ValidateHeaderKey())
            {
                if (ModelState.IsValid)
                {
                    await this._iactService.Deposit(resource);
                }
                else
                {
                    resource.Response.ErrorMessage = "Model state is unvalid";
                }
            }
            else
            {
                resource.Response.ErrorMessage = "Authen Fail.";
            }
            return StatusCode(resource.Response.IsError ? 400 : 200, resource);
        }


        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] AccountResource resource)
        {
            if (this.ValidateHeaderKey())
            {
                if (ModelState.IsValid)
                {
                    await this._iactService.Transfer(resource);
                }
                else
                {
                    resource.Response.ErrorMessage = "Model state is unvalid";
                }
            }
            else
            {
                resource.Response.ErrorMessage = "Authen Fail.";
            }
            return StatusCode(resource.Response.IsError ? 400 : 200, resource);

        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccount()
        {
            var acts = new List<Account>();
            if (this.ValidateHeaderKey())
            {
                acts = await this._iactService.GetAccounts();
            }
            return StatusCode(200, acts);
        }
    }
}
