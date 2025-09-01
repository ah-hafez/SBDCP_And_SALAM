using MCS.Domain;
using MCS.DTO.Tenants;

namespace MCS.Tenants.Service.Mappers
{
    public static class UserTenantMapper
    {
        public static UserTenant ToUserTenant(this UserTenantDTO model)
        {
            if (model == null) return new UserTenant();
            UserTenant oUserTenant = new UserTenant();
            oUserTenant.Id = model.Id;
            oUserTenant.TenantId = model.TenantId;
            oUserTenant.UserName = model.UserName;
            oUserTenant.Tenant = model.Tenant?.ToTenant();

            return oUserTenant;
        }

        public static UserTenantDTO ToTenantDTO(this UserTenant model)
        {
            if (model == null) return new UserTenantDTO();
            UserTenantDTO oUserTenantDTO = new UserTenantDTO();
            oUserTenantDTO.Id = model.Id;
            oUserTenantDTO.UserName = model.UserName;
            oUserTenantDTO.TenantId = model.TenantId;
            oUserTenantDTO.Tenant = model.Tenant?.ToTenantDTO();
            return oUserTenantDTO;
        }
    }
}