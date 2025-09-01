using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YESSER.NCS.MCM.Domain;
using YESSER.NCS.MCM.DTO.Tenant;

namespace YESSER.NCS.MCM.Tenants.Service.Mappers
{
    public static class TenantMapperNew
    {
        public static Tenant Map(AddTenantDTO AddTenantDTO)
        {
            Tenant tenant = new Tenant()
            {
                Name = LocalizationIdentifierMapper.MapTenant(AddTenantDTO.Names),
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

            };
            return tenant;
        }

        public static Tenant MapTenant(EditTenantDTO EditTenantDTO)
        {
            Tenant tenant = new Tenant()
            {
                ID = EditTenantDTO.Id,
                Name = LocalizationIdentifierMapper.MapTenant(EditTenantDTO.Names),
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
            };

            return tenant;
        }
        //public static EditTenantVM Map(EditTenantDTO EditTenantDTO)
        //{
        //    EditTenantVM tenant = new EditTenantVM()
        //    {
        //        Id = EditTenantDTO.Id,
        //        Names = LocalizationIdentifierMapper.Map(EditTenantDTO.Names),
        //        OrgUnitsCount = EditTenantDTO.OrgUnitsCount,
        //        UsersCount = EditTenantDTO.UsersCount,
        //        ToDate = EditTenantDTO.ToDate,
        //        ToDateH = EditTenantDTO.ToDateH,
        //        FromDate = EditTenantDTO.FromDate,
        //        FromDateH = EditTenantDTO.FromDateH,
        //        DelegatedName = LocalizationIdentifierMapper.Map(EditTenantDTO.DelegatedName),
        //        DelegatedUserName = EditTenantDTO.DelegatedUserName,
        //        DelegatedMobile = EditTenantDTO.DelegatedMobile,
        //        DelegatedEmail = EditTenantDTO.DelegatedEmail,
        //        HostName = EditTenantDTO.HostName,
        //    };

        //    return tenant;
        //}

        public static EditTenantDTO Map(Tenant tenant)
        {
            EditTenantDTO EditTenantDTO = new EditTenantDTO()
            {
                Id = tenant.ID,
                Names = LocalizationIdentifierMapper.MapTenant(tenant.Name.Localizations),
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
                    tenantDTOs.Add(TenantMapperNew.MapTenant(tenant));
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
                Id = tenant.ID,
                LocalName = tenant.LocalName,
                LocalDelegatedName = tenant.LocalDelegatedName,
                UsersCount = tenant.UsersCount,
                ToDate = tenant.ToDate,
                ToDateH = tenant.ToDateH,
                FromDateH = tenant.FromDateH,
                DelegatedUserName = tenant.DelegatedUserName,
                DelegatedMobile = tenant.DelegatedMobile,
                DelegatedEmail = tenant.DelegatedEmail,
                HostName = tenant.HostName,
                IsActive = tenant.IsActive,
            };

            return tenantDTO;
        }
        //public static List<TenantVM> Map(List<TenantDTO> tenantDTOs)
        //{
        //    if (tenantDTOs == null || !tenantDTOs.Any())
        //    {
        //        return null;
        //    }

        //    List<TenantVM> tenantVMs = tenantDTOs.Select(tenantDTO => new TenantVM()
        //    {
        //        Id = tenantDTO.Id,
        //        LocalName = tenantDTO.LocalName,
        //        LocalDelegatedName = tenantDTO.LocalDelegatedName,
        //        OrgUnitsCount = tenantDTO.OrgUnitsCount,
        //        UsersCount = tenantDTO.UsersCount,
        //        ToDate = tenantDTO.ToDate,
        //        ToDateH = tenantDTO.ToDateH,
        //        FromDateH = tenantDTO.FromDateH,
        //        DelegatedUserName = tenantDTO.DelegatedUserName,
        //        DelegatedMobile = tenantDTO.DelegatedMobile,
        //        DelegatedEmail = tenantDTO.DelegatedEmail,
        //        HostName = tenantDTO.HostName,
        //        IsActive = tenantDTO.IsActive,
        //    }).ToList();

        //    return tenantVMs;

        //}
    }
}