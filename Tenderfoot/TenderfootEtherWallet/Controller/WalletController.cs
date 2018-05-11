using Microsoft.AspNetCore.Mvc;
using Tenderfoot.Mvc;
using TenderfootEtherWallet.Models.Wallet;

namespace TenderfootEtherWallet.Controller
{
    [Route("api/wallet")]
    public class WalletController : TfController
    {
        [HttpPost]
        [Route("token/validate")]
        public JsonResult AddTokenValidate()
        {
            this.Initiate<TokenModel>();
            return this.Validate();
        }

        [HttpPost]
        [Route("token")]
        public JsonResult AddToken()
        {
            this.Initiate<TokenModel>();
            return this.Conclude();
        }

        [HttpGet]
        [Route("token")]
        public JsonResult GetToken()
        {
            this.Initiate<TokenModel>();
            return this.Conclude();
        }
    }
}
