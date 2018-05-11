using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootEtherWallet.DB;

namespace TenderfootEtherWallet.Models.Member
{
    public class MemberModel : TfModel
    {
        private Schema<Members> Members { get; set; } = _DB.Members;
        private Schema<TokenWallets> TokenWallets { get; set; } = _DB.TokenWallets;
        
        [Input]
        [JsonProperty]
        public Members Member { get; set; }

        [JsonProperty]
        public TokenWallets TokenWallet { get; set; }

        public override IEnumerable<ValidationResult> Validate()
        {
            if (this.Mapping)
            {
                yield return this.ValidateSession();
                if (this.IsValidSession())
                {
                    this.Members.Entity.username = Encryption.Decrypt(this.SessionId);
                    if (!this.Members.HasRecord)
                    {
                        yield return TfValidationResult.Compose("DataNotFound", nameof(this.SessionId));
                    }
                }
            }

            if (this.Handling)
            {
                if (this.IsValid(this.Member.GetInputColumns().ToArray()))
                {
                    this.Members.Case.Where(this.Members._(x => x.username), Is.EqualTo, this.Member.username);
                    this.Members.Case.Where(Operator.OR, this.Members._(x => x.email), Is.EqualTo, this.Member.email);
                    if (this.Members.HasRecords)
                    {
                        yield return TfValidationResult.Compose("InvalidDuplicate", nameof(this.Member.username), nameof(this.Member.email));
                    }
                }
            }
        }

        public override void MapModel()
        {
            this.Members.Entity.password = null;
            this.Member = this.Members.SelectToEntity();

            this.TokenWallets.Entity.member_id = this.Members.Entity.id;
            this.TokenWallet = this.TokenWallets.SelectToEntity();
        }

        public override void HandleModel()
        {
            this.Members.Entity.SetValuesFromModel(this.Member);
            this.Members.Insert();
        }
    }
}