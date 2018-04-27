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
    }
}
