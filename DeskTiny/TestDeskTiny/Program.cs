using DeskTiny.Database;
using System;

namespace TestDeskTiny
{
    class Program
    {
        static void Main(string[] args)
        {
            var affiliate = new DeskTiny.Database.Tables.Affiliate();

            affiliate.QueryConditions.AddWhere("site_id", Condition.Equal, 1);
            affiliate.QueryConditions.AddWhere("active", Condition.Equal, 1);
            affiliate.QueryConditions.EndWhere(Operator.OR);
            affiliate.QueryConditions.AddWhere("site_id", Condition.Equal, 0);
            affiliate.QueryConditions.AddWhere("active", Condition.Equal, 0);
            affiliate.QueryConditions.AddLimit(5);
            affiliate.Select();

            affiliate.ClearQueryConditions();

            affiliate.Entity.business_client_code = "businesscode_pao125";
            affiliate.Entity.pr_tfaf = "testaffiliate_pao125";

            affiliate.Insert();
            affiliate.ClearEntity();

            Console.ReadKey();
        }
    }
}
