using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public interface ITenantBL
    {
        int AddTenant(Tenant tenant, string cultureName);
        void UpdateTenant(Tenant tenant);
        Tenant GetTenantById(int tenantId);
        Tenant GetTenantByUserName(string userName, string CultureName, bool validate = true);
        Tenant GetTenantByHostName(string hostName, bool validate = true);
        Tenant GetTenantById(int tenantId, bool validate);
        IList<Tenant> GetTenants(Expression<Func<Tenant, bool>> @where, string cultureName);
        IList<Tenant> GetAllTenants(string cultureName);
        void DeleteTenants(IList<int> Ids);
        IList<Tenant> GetTenants(SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        IList<TenantCulture> GetTenantCultures();
        TenantLookup GetLookupItem(int lookupId);
        void Validate(Tenant tenant);
        void ActivateTenant(int tenantId, bool isActive);
        Tenant GetTenantByYesserCode(string yesserCode);
        List<TenantNotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType);
        void SaveNotification(TenantNotification tenantNotification);
        void SendSupportEmail(SupportDTO supportDTO, string cultureName);
        void UpdateNotifactionDetails(IList<TenantNotificationDetail> tenantNotificationDetail);
        UserTenant GetUserTenantById(int id);
    }
}
