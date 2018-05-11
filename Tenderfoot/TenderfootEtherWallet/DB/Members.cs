using Tenderfoot.Database;
using Tenderfoot.Mvc;

namespace TenderfootEtherWallet.DB
{
    public class Members : Entity
    {
        [Input]
        [RequireInput(Method.POST)]
        [ValidateInput(InputType.Alphabet)]
        [NotNull]
        public string username { get; set; }

        [Input]
        [RequireInput(Method.POST)]
        [ValidateInput(InputType.Email)]
        [NotNull]
        public string email { get; set; }

        [Input]
        [RequireInput(Method.POST)]
        [ValidateInput(InputType.String)]
        [NotNull]
        public string password { get; set; }
    }
}