namespace MCS.DTO.Tenants
{
    public class TenantLookupLocalizationDTO: BaseDTO
    {
        public TenantLookupDTO Lookup { get; set; }
        public TenantCultureDTO Culture { get; set; }
        public string Text { get; set; }
    }
}
