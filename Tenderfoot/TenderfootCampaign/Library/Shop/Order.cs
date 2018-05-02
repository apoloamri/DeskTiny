using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootCampaign.Library._Common;
using TenderfootCampaign.Library._Database;
using TenderfootCampaign.Library.Campaign;

namespace TenderfootCampaign.Library.Shop
{
    public class Order
    {
        public Members Member { get; set; }
        public Schema<Carts> Carts { get; set; } = _DB.Carts;
        public List<dynamic> CartItems { get; set; }

        private int TotalPoints { get; set; }
        private int TotalPrice { get; set; }

        public ValidationResult ValidateCart(params string[] memberNames)
        {
            if (!this.Carts.HasRecords)
            {
                return TfValidationResult.Compose("EmptyCart", memberNames);
            }
            return null;
        }

        public ValidationResult ValidateWallet(string sessionId, params string[] memberNames)
        {
            var wallet = MemberStrategy.GetMemberWallet(this.Member.id);
            if (this.TotalPrice > wallet.amount)
            {
                return TfValidationResult.Compose("InsufficientCash", memberNames);
            }
            return null;
        }

        public void GetCartItems()
        {
            var items = _DB.Items;
            var sessions = _DB.Sessions;
            var members = _DB.Members;

            this.Carts.Relate(Join.INNER, items,
                items.Relation(items._(x => x.item_code), this.Carts._(x => x.item_code)));

            this.Carts.Relate(Join.INNER, sessions,
                sessions.Relation(sessions._(x => x.session_key), this.Carts._(x => x.session_key)));

            this.Carts.Relate(Join.INNER, members,
                members.Relation(members._(x => x.username), sessions._(x => x.session_id)));
            
            this.CartItems = this.Carts.Select.Result;
            this.TotalPrice = this.CartItems.Sum(x => (int)x.price * (int)x.amount);
            this.TotalPoints =
                (int)Math.Ceiling(this.TotalPrice * (Convert.ToDouble(percent) / 100));
        }

        public void ApplyDiscounts(string sessionId)
        {
            var bonus = new GetBonus();
            bonus.GetCampaignCode(sessionId);
            var currentLevel = bonus.GetCurrentLevel();
            if (currentLevel == null)
            {
                return;
            }
            this.TotalPoints = 
                (int)Math.Ceiling(this.TotalPoints + (this.TotalPoints * (Convert.ToDouble(currentLevel["multiplier"]) / 100)));

            this.TotalPrice =
                (int)Math.Ceiling(this.TotalPrice - (this.TotalPrice * (Convert.ToDouble(currentLevel["discount"]) / 100)));
        }

        public void DeleteCart()
        {
            this.Carts.Delete();
        }

        public void ExecuteOrder()
        {
            this.InsertOrderHeaders(out int orderId, out int totalPoints);
            this.InsertOrders(orderId);
        }
        
        private int percent = Convert.ToInt32(TfSettings.GetSettings("Campaign", "PointPercent"));

        private void InsertOrderHeaders(out int orderId, out int totalPoints)
        {
            var orderHeader = _DB.OrderHeaders;

            orderHeader.Entity.member_id = this.Member.id;
            orderHeader.Entity.price = this.TotalPrice;
            orderHeader.Entity.points = this.TotalPoints;
            orderHeader.Insert();

            orderId = orderHeader.Entity.id ?? 0;
            totalPoints = orderHeader.Entity.points ?? 0;

            TokensStrategy.Add(this.Member.id, totalPoints);
            MemberStrategy.UpdateMemberWallet(this.Member.id, -this.TotalPrice);
        }

        private void InsertOrders(long orderId)
        {
            var orders = _DB.Orders;
            
            foreach (var item in this.CartItems)
            {
                var total = (int)item.price * (int)item.amount;
                var points = (total / 100) * percent;

                orders.Entity.header_id = orderId;
                orders.Entity.item_code = item.item_code;
                orders.Entity.amount = (int)item.amount;
                orders.Entity.total_price = total;
                orders.Entity.points_earned = points;
                orders.Insert();
            }
        }
    }
}
