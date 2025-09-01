using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class TenantMapper
    {
        public static Tenant Map(AddTenantDTO tenantAddDTO)
        {
            if (tenantAddDTO == null)
            {
                return null;
            }
            Tenant tenant = new Tenant()
            {
                Name = LocalizationIdentifierMapper.MapTenant(tenantAddDTO.Names),
                OrgUnitsCount = tenantAddDTO.OrgUnitsCount,
                UsersCount = tenantAddDTO.UsersCount,
                ToDate = tenantAddDTO.ToDate,
                FromDate = tenantAddDTO.FromDate,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(tenantAddDTO.DelegatedName),
                DelegatedUserName = tenantAddDTO.DelegatedUserName,
                DelegatedMobile = tenantAddDTO.DelegatedMobile,
                DelegatedEmail = tenantAddDTO.DelegatedEmail,
            };

            return tenant;
        }

        public static Tenant Map(EditTenantDTO tenantEditDTO)
        {
            if (tenantEditDTO == null)
            {
                return null;
            }
            Tenant tenant = new Tenant()
            {
                Id = tenantEditDTO.Id,
                Name = LocalizationIdentifierMapper.MapTenant(tenantEditDTO.Names),
                OrgUnitsCount = tenantEditDTO.OrgUnitsCount,
                UsersCount = tenantEditDTO.UsersCount,
                ToDate = tenantEditDTO.ToDate,
                FromDate = tenantEditDTO.FromDate,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(tenantEditDTO.DelegatedName),
                DelegatedUserName = tenantEditDTO.DelegatedUserName,
                DelegatedMobile = tenantEditDTO.DelegatedMobile,
                DelegatedEmail = tenantEditDTO.DelegatedEmail,
            };

            return tenant;
        }

        public static EditTenantDTO Map(Tenant tenant)
        {
            if (tenant == null)
            {
                return null;
            }
            EditTenantDTO tenantEditDTO = new EditTenantDTO()
            {
                Id = tenant.Id,
                Names = LocalizationIdentifierMapper.MapTenant(tenant.Name.Localizations),
                OrgUnitsCount = tenant.OrgUnitsCount,
                UsersCount = tenant.UsersCount,
                ToDate = tenant.ToDate,
                FromDate = tenant.FromDate,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(tenant.DelegatedName.Localizations),
                DelegatedUserName = tenant.DelegatedUserName,
                DelegatedMobile = tenant.DelegatedMobile,
                DelegatedEmail = tenant.DelegatedEmail,
            };

            return tenantEditDTO;
        }

        public static TenantDTO MapTenant(Tenant tenant)
        {
            if (tenant == null)
            {
                return null;
            }
            TenantDTO tenantEditDTO = new TenantDTO()
            {
                Id = tenant.Id,
                OrgUnitsCount = tenant.OrgUnitsCount,
                UsersCount = tenant.UsersCount,
                ToDate = tenant.ToDate,
                FromDate = tenant.FromDate,
                DatabaseName = tenant.DatabaseName,
                DelegatedUserName = tenant.DelegatedUserName,
                DelegatedMobile = tenant.DelegatedMobile,
                DelegatedEmail = tenant.DelegatedEmail,
                Logo = tenant.Logo,
                LocalName = tenant.LocalName
            };

            return tenantEditDTO;
        }

        public static List<TenantDTO> Map(IList<Tenant> tenants)
        {
            if (tenants == null || !tenants.Any())
            {
                return null;
            }
            List<TenantDTO> tenantDTOs = tenants
                .Select(tenant => new TenantDTO()
                {
                    Id = tenant.Id,
                    LocalName = tenant.LocalName,
                    LocalDelegatedName = tenant.LocalDelegatedName,
                    OrgUnitsCount = tenant.OrgUnitsCount,
                    UsersCount = tenant.UsersCount,
                    ToDate = tenant.ToDate,
                    FromDate = tenant.FromDate,
                    DelegatedUserName = tenant.DelegatedUserName,
                    DelegatedMobile = tenant.DelegatedMobile,
                    DelegatedEmail = tenant.DelegatedEmail,
                }).ToList();
            return tenantDTOs;
        }

    }
}
