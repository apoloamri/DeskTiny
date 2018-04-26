﻿using Tenderfoot.Database;

namespace TenderfootCampaign.Library._Database
{
    public class _DB : Schemas
    {
        public static Schema<Bonus> Bonus => CreateTable<Bonus>("bonus");
        public static Schema<Carts> Carts => CreateTable<Carts>("carts");
        public static Schema<Campaign> Campaign => CreateTable<Campaign>("campaign");
        public static Schema<Items> Items => CreateTable<Items>("items");
        public static Schema<Members> Members => CreateTable<Members>("members");
        public static Schema<OrderHeaders> OrderHeaders => CreateTable<OrderHeaders>("order_headers");
        public static Schema<Orders> Orders => CreateTable<Orders>("orders");
        public static Schema<Tokens> Tokens => CreateTable<Tokens>("tokens");
    }
}
