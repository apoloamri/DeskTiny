using Tenderfoot.Database;
using Tenderfoot.TfSystem;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library._Common
{
    public static class TokensStrategy
    {
        public static void Add(int? memberId, int? points)
        {
            var tokens = _DB.Tokens;
            tokens.Entity.member_id = memberId;
            tokens.Case.OrderBy(tokens._(x => x.id), Order.DESC);
            tokens.Case.LimitBy(1);
            var latestToken = tokens.Select.Entity;
            if (latestToken != null)
            {
                tokens.Entity.SetValuesFromModel(latestToken);
            }
            tokens.Entity.add_points = points;
            tokens.Entity.total_points = (latestToken?.total_points ?? 0) + points;
            tokens.Insert();
        }
        
        public static int GetPoints(string sessionId)
        {
            var tokens = _DB.Tokens;
            var members = _DB.Members;

            tokens.Relate(Join.INNER, members,
                members.Relation(members._(x => x.id), tokens._(x => x.member_id)));

            tokens.Case.Where(members._(x => x.username), Is.EqualTo, Encryption.Decrypt(sessionId));
            tokens.Case.OrderBy(tokens._(x => x.id), Order.DESC);
            tokens.Case.LimitBy(1);
            return tokens.SelectToEntity().total_points ?? 0;
        }
    }
}
