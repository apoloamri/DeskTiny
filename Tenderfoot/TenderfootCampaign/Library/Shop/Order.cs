using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tenderfoot.Database;
using Tenderfoot.Mvc;
using Tenderfoot.TfSystem;
using TenderfootCampaign.Library._Common;
using TenderfootCampaign.Library._Database;

namespace TenderfootCampaign.Library.Shop
{
    public class Order
    {
        public Schema<Carts> Carts { get; set; } = _DB.Carts;
        public List<dynamic> CartItems { get; set; }

        public ValidationResult ValidateCart(params string[] memberNames)
        {
            if (!this.Carts.HasRecords)
            {
                return TfValidationResult.Compose("EmptyCart", memberNames);
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
            var memberId = (int?)this.CartItems.FirstOrDefault()?.id3;
            var orderHeader = _DB.OrderHeaders;

            orderHeader.Entity.member_id = memberId;
            orderHeader.Entity.price = this.CartItems.Sum(x => (int)x.price * (int)x.amount);
            orderHeader.Entity.points = this.CartItems.Sum(x => (((int)x.price * (int)x.amount) / 100) * percent);
            orderHeader.Insert();

            orderId = orderHeader.Entity.id ?? 0;
            totalPoints = orderHeader.Entity.points ?? 0;

            TokensStrategy.Add(memberId, totalPoints);
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
