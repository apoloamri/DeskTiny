using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Shop
{
    public class Order
    {
        public Schema<Carts> Carts { get; set; } = _DB.Carts;
        public List<dynamic> CartItems { get; set; }

        public void GetCartItems()
        {
            var items = _DB.Items;
            var sessions = _DB.Sessions;
            var members = _DB.Members;

            this.Carts.Relate(Join.INNER, items,
                items.Relation(items._("item_code"), this.Carts._("item_code")));

            this.Carts.Relate(Join.INNER, sessions,
                sessions.Relation(sessions._("session_key"), this.Carts._("session_key")));

            this.Carts.Relate(Join.INNER, members,
                members.Relation(members._("username"), sessions._("session_id")));
            
            this.CartItems = this.Carts.Select.Result;
        }

        public ValidationResult ValidateCart(params string[] memberNames)
        {
            if (!this.Carts.HasRecords)
            {
                return TfValidationResult.Compose("EmptyCart", memberNames);
            }
            return null;
        }
    }
}
