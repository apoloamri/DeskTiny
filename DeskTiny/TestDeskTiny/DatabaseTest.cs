using DeskTiny.Database;

namespace TestDeskTiny
{
    public static class DatabaseTest
    {
        public static void TestAll()
        {
            //TestInsert();
            TestSelect();
            //TestUpdate();
            //TestDelete();
        }

        private static void TestSelect()
        {
            var affiliate = Schemas.Affiliate();

            affiliate.QueryConditions.AddWhere("site_id", Condition.Equal, 1);
            affiliate.QueryConditions.AddWhere("active", Condition.Equal, 1);
            affiliate.QueryConditions.EndWhere(Operator.OR);
            affiliate.QueryConditions.AddWhere("site_id", Condition.Equal, 0);
            affiliate.QueryConditions.AddWhere("active", Condition.Equal, 0);
            affiliate.QueryConditions.AddLimit(5);
            
            var result = affiliate.Select.Dictionaries();

            affiliate.ClearQueryConditions();
        }

        private static void TestInsert()
        {
            var affiliate = Schemas.Affiliate();

            affiliate.Entity.business_client_code = "businesscode_pao1";
            affiliate.Entity.pr_tfaf = "testaffiliate_pao1";
            affiliate.Insert();
            affiliate.ClearEntity();

            affiliate.Entity.business_client_code = "businesscode_pao2";
            affiliate.Entity.pr_tfaf = "testaffiliate_pao2";
            affiliate.Insert();
            affiliate.ClearEntity();
        }

        private static void TestUpdate()
        {
            var affiliate = Schemas.Affiliate();

            affiliate.Entity.agency_identifier = "updated";
            affiliate.QueryConditions.AddWhere("business_client_code", Condition.Equal, "businesscode_pao1", Operator.OR);
            affiliate.QueryConditions.AddWhere("business_client_code", Condition.Equal, "businesscode_pao2");
            affiliate.Update();
            affiliate.ClearEntity();
        }

        private static void TestDelete()
        {
            var affiliate = Schemas.Affiliate();

            affiliate.QueryConditions.AddWhere("business_client_code", Condition.Equal, "businesscode_pao1", Operator.OR);
            affiliate.QueryConditions.AddWhere("business_client_code", Condition.Equal, "businesscode_pao2");
            affiliate.Delete();
            affiliate.ClearEntity();
        }
    }
}
