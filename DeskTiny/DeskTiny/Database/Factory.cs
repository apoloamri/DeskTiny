namespace DeskTiny.Database
{
    public class Factory
    {
        public Tables.Affiliate GetAffiliate()
        {
            return new Tables.Affiliate();
        }
    }
}
