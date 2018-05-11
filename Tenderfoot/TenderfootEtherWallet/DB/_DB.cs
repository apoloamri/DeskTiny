using Tenderfoot.Database;

namespace TenderfootEtherWallet.DB
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<TokenWallets> TokenWallets => CreateTable<TokenWallets>("token_wallets");
    }
}
