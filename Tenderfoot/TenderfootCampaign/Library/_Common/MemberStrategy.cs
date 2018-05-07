using Tenderfoot.TfSystem;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library._Common
{
    public static class MemberStrategy
    {
        public static Members GetMemberFromSession(string sessionId)
        {
            var member = _DB.Members;
            member.Entity.username = Encryption.Decrypt(sessionId);
            member.Entity.active = 1;
            return member.Select.Entity;
        }

        public static Wallets GetMemberWallet(int? memberId)
        {
            var wallets = _DB.Wallets;
            wallets.Entity.member_id = memberId;
            return wallets.SelectToEntity();
        }

        public static void UpdateMemberWallet(int? memberId, int amount)
        {
            var wallets = _DB.Wallets;
            wallets.Entity.member_id = memberId;
            if (wallets.HasRecord)
            {
                wallets.SelectToEntity();
                wallets.Entity.amount = wallets.Entity.amount + amount;
                wallets.Update();
            }
            else
            {
                wallets.Entity.amount = amount;
                wallets.Insert();
            }
        }
    }
}
