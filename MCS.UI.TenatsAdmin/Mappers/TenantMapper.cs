using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Tenants;
using MCS.UI.TenantsAdmin.Models.Tenant;

namespace MCS.UI.TenantsAdmin.Mappers
{
    public static class TenantMapper
    {
        public static Tenant Map(AddTenantDTO AddTenantDTO)
        {
            Tenant tenant = new Tenant()
            {
                Name = LocalizationIdentifierMapper.MapTenant(AddTenantDTO.Names),
                OrgUnitsCount = AddTenantDTO.OrgUnitsCount,
                UsersCount = AddTenantDTO.UsersCount,
                ToDate = AddTenantDTO.ToDate,
                ToDateH = AddTenantDTO.ToDateH,
                FromDate = AddTenantDTO.FromDate,
                FromDateH = AddTenantDTO.FromDateH,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(AddTenantDTO.DelegatedName),
                DelegatedUserName = AddTenantDTO.DelegatedUserName,
                DelegatedMobile = AddTenantDTO.DelegatedMobile,
                DelegatedEmail = AddTenantDTO.DelegatedEmail,
                HostName = AddTenantDTO.HostName,
                Logo = AddTenantDTO.Logo,
                YesserCertificate = AddTenantDTO.YesserCertificate
            };
            return tenant;
        }

        public static Tenant MapTenant(EditTenantDTO EditTenantDTO)
        {
            Tenant tenant = new Tenant()
            {
                Id = EditTenantDTO.Id,
                Name = LocalizationIdentifierMapper.MapTenant(EditTenantDTO.Names),
                OrgUnitsCount = EditTenantDTO.OrgUnitsCount,
                UsersCount = EditTenantDTO.UsersCount,
                ToDate = EditTenantDTO.ToDate,
                ToDateH = EditTenantDTO.ToDateH,
                FromDate = EditTenantDTO.FromDate,
                FromDateH = EditTenantDTO.FromDateH,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(EditTenantDTO.DelegatedName),
                DelegatedUserName = EditTenantDTO.DelegatedUserName,
                DelegatedMobile = EditTenantDTO.DelegatedMobile,
                DelegatedEmail = EditTenantDTO.DelegatedEmail,
                HostName = EditTenantDTO.HostName,
                Logo = EditTenantDTO.Logo,
                YesserCertificate = EditTenantDTO.YesserCertificate
            };

            return tenant;
        }
        public static EditTenantVM Map(EditTenantDTO EditTenantDTO)
        {
            EditTenantVM tenant = new EditTenantVM()
            {
                Id = EditTenantDTO.Id,
                Names = LocalizationIdentifierMapper.Map(EditTenantDTO.Names),
                OrgUnitsCount = EditTenantDTO.OrgUnitsCount,
                UsersCount = EditTenantDTO.UsersCount,
                ToDate = EditTenantDTO.ToDate,
                ToDateH = EditTenantDTO.ToDateH,
                FromDate = EditTenantDTO.FromDate,
                FromDateH = EditTenantDTO.FromDateH,
                DelegatedName = LocalizationIdentifierMapper.Map(EditTenantDTO.DelegatedName),
                DelegatedUserName = EditTenantDTO.DelegatedUserName,
                DelegatedMobile = EditTenantDTO.DelegatedMobile,
                DelegatedEmail = EditTenantDTO.DelegatedEmail,
                HostName = EditTenantDTO.HostName,
                Logo = EditTenantDTO.Logo,
                YesserCertificate = EditTenantDTO.YesserCertificate
            };

            return tenant;
        }
        public static EditTenantVM Map(TenantDTO tenantDTO)
        {
            EditTenantVM tenant = new EditTenantVM()
            {
                Id = tenantDTO.Id,
                Names = LocalizationIdentifierMapper.Map(tenantDTO.Name),
                OrgUnitsCount = tenantDTO.OrgUnitsCount,
                UsersCount = tenantDTO.UsersCount,
                ToDate = tenantDTO.ToDate,
                ToDateH = tenantDTO.ToDateH,
                FromDate = tenantDTO.FromDate,
                FromDateH = tenantDTO.FromDateH,
                DelegatedName = LocalizationIdentifierMapper.Map(tenantDTO.DelegatedName),
                DelegatedUserName = tenantDTO.DelegatedUserName,
                DelegatedMobile = tenantDTO.DelegatedMobile,
                DelegatedEmail = tenantDTO.DelegatedEmail,
                HostName = tenantDTO.HostName,
                Logo = tenantDTO.Logo,
                YesserCertificate = tenantDTO.YesserCertificate
            };

            return tenant;
        }
        public static EditTenantDTO Map(Tenant tenant)
        {
            EditTenantDTO EditTenantDTO = new EditTenantDTO()
            {
                Id = tenant.Id,
                Names = LocalizationIdentifierMapper.MapTenant(tenant.Name.Localizations),
                OrgUnitsCount = tenant.OrgUnitsCount,
                UsersCount = tenant.UsersCount,
                ToDate = tenant.ToDate,
                ToDateH = tenant.ToDateH,
                FromDate = tenant.FromDate,
                FromDateH = tenant.FromDateH,
                DelegatedName = LocalizationIdentifierMapper.MapTenant(tenant.DelegatedName.Localizations),
                DelegatedUserName = tenant.DelegatedUserName,
                DelegatedMobile = tenant.DelegatedMobile,
                DelegatedEmail = tenant.DelegatedEmail,
                HostName = tenant.HostName,
                Logo = tenant.Logo,
                YesserCertificate = tenant.YesserCertificate
            };

            return EditTenantDTO;
        }

        public static List<TenantDTO> Map(IList<Tenant> tenants)
        {
            List<TenantDTO> tenantDTOs = new List<TenantDTO>();

            if (tenants != null)
            {
                foreach (Tenant tenant in tenants)
                {
                    tenantDTOs.Add(TenantMapper.MapTenant(tenant));
                }
            }

            return tenantDTOs;
        }

        public static TenantDTO MapTenant(Tenant tenant)
        {
            if (tenant == null)
            {
                return null;
            }

            TenantDTO tenantDTO = new TenantDTO()
            {
                Id = tenant.Id,
                LocalName = tenant.LocalName,
                LocalDelegatedName = tenant.LocalDelegatedName,
                OrgUnitsCount = tenant.OrgUnitsCount,
                UsersCount = tenant.UsersCount,
                ToDate = tenant.ToDate,
                ToDateH = tenant.ToDateH,
                FromDateH = tenant.FromDateH,
                DelegatedUserName = tenant.DelegatedUserName,
                DelegatedMobile = tenant.DelegatedMobile,
                DelegatedEmail = tenant.DelegatedEmail,
                HostName = tenant.HostName,
                IsActive = tenant.IsActive,
                Logo = tenant.Logo,
                YesserCertificate = tenant.YesserCertificate
            };

            return tenantDTO;
        }
        public static List<TenantVM> Map(List<TenantDTO> tenantDTOs)
        {
            if (tenantDTOs == null || !tenantDTOs.Any())
            {
                return null;
            }

            List<TenantVM> tenantVMs = tenantDTOs.Select(tenantDTO => new TenantVM()
            {
                Id = tenantDTO.Id,
                LocalName = tenantDTO.Name.Localizations.FirstOrDefault().Text,
                LocalDelegatedName = tenantDTO.LocalDelegatedName,
                OrgUnitsCount = tenantDTO.OrgUnitsCount,
                UsersCount = tenantDTO.UsersCount,
                ToDate = tenantDTO.ToDate,
                ToDateH = tenantDTO.ToDateH,
                FromDateH = tenantDTO.FromDateH,
                DelegatedUserName = tenantDTO.DelegatedUserName,
                DelegatedMobile = tenantDTO.DelegatedMobile,
                DelegatedEmail = tenantDTO.DelegatedEmail,
                HostName = tenantDTO.HostName,
                IsActive = tenantDTO.IsActive,
                Logo = tenantDTO.Logo,
                YesserCertificate = tenantDTO.YesserCertificate
            }).ToList();

            return tenantVMs;

        }

        public static List<UserTenantVM> Map(List<UserTenantDTO> userTenantDTOs, string cultureName)
        {
            if (userTenantDTOs == null || !userTenantDTOs.Any())
            {
                return new List<UserTenantVM>();
            }

            List<UserTenantVM> tenantVMs = userTenantDTOs.Select(tenantDTO => new UserTenantVM()
            {
                Id = tenantDTO.Id,
                UserName = tenantDTO.UserName,
                LocalName = tenantDTO.Tenant.Name.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text
            }).ToList();

            return tenantVMs;
        }
        public static UserTenantVM Map(UserTenantDTO userTenantDTO)
        {
            if (userTenantDTO == null)
            {
                return new UserTenantVM();
            }

            var tenantDTO = new UserTenantVM()
            {
                Id = userTenantDTO.Id,
                UserName = userTenantDTO.UserName,
                TenantId = userTenantDTO.Tenant.Id,
            };

            return tenantDTO;
        }
    }
}
