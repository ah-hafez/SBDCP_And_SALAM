namespace MCS.DTO.Tenants
{
    public class UserTenantDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int TenantId { get; set; }
        public TenantDTO Tenant { get; set; }
    }
}
