using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootEtherWallet.DB;

namespace TenderfootEtherWallet.Models.Wallet
{
    public class TokenModel : TfModel
    {
        [Input]
        [Required]
        public TokenWallets Wallet { get; set; }

        [JsonProperty]
        public List<dynamic> Wallets { get; set; }

        private Schema<TokenWallets> TokenWallets { get; set; } = _DB.TokenWallets;

        public override IEnumerable<ValidationResult> Validate()
        {
            yield return this.ValidateSession();
            if (this.Handling)
            {
                if (this.IsValid(this.Wallet.GetInputColumns().ToArray()))
                {
                    this.TokenWallets.Entity.token_address = this.Wallet.token_address;
                    this.TokenWallets.Entity.address = this.Wallet.address;
                    if (this.TokenWallets.HasRecord)
                    {
                        var items = $"{this.Wallet.token_address}, {this.Wallet.address}";
                        yield return TfValidationResult.Compose("InvalidDuplicate", new[] { items }, nameof(this.TokenWallets));
                    }
                }
            }
        }

        public override void HandleModel()
        {
            this.TokenWallets.Entity.member_id = this.GetMemberId();
            this.TokenWallets.Entity.token_address = Convert.ToString(TfSettings.GetSettings("EtherWallet", "ContractAddress"));
            this.TokenWallets.Entity.SetValuesFromModel(this.Wallet, false);
            this.TokenWallets.Insert();
        }

        public override void MapModel()
        {
            this.TokenWallets.Entity.member_id = this.GetMemberId();
            this.Wallets = this.TokenWallets.Select.Result;
        }

        private int? GetMemberId()
        {
            var members = _DB.Members;
            members.Entity.username = Encryption.Decrypt(this.SessionId);
            return members.SelectToEntity().id;
        }
    }
}