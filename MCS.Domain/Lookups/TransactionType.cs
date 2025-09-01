namespace MCS.Domain
{
    public class TransactionType : LookupBase
    {
        public virtual LocalizationIdentifier Abbreviation { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Lookup Color { get; set; }
    }
}
