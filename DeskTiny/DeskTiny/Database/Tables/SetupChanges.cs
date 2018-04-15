namespace DTCore.Database.Tables
{
    public class SetupChanges : Entity
    {
        [NotNull]
        [Default("0")]
        public bool Encrypted { get; set; }
    }
}
