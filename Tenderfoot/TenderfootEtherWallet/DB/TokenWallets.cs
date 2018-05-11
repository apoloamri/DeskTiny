using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace TenderfootEtherWallet.DB
{
    public class TokenWallets : Entity
    {
        [NotNull]
        public int? member_id { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string address { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string token_address { get; set; }

        [Input]
        [RequireInput]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string wallet_name { get; set; }
    }
}