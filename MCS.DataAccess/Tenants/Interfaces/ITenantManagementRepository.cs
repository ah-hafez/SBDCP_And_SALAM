using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITenantManagementRepository
    {
        int AddTenant(Tenant tenant);
        void UpdateTenant(Tenant tenant);
        Tenant GetTenantById(int tenantId);
        Tenant GetTenantByHostName(string hostName);
        Tenant GetTenantByUserName(string userName, string cultureName);
        Tenant GetTenantById(int tenantId, bool validate);
        IList<Tenant> GetTenants(Expression<Func<Tenant, bool>> @where, string cultureName);
        IList<Tenant> GetAllTenants(string cultureName);
        IList<Tenant> GetTenants(SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        TenantCulture GetTenantCulture(int cultureId);
        //void GenerateDatabase(string script, string databaseProvider);
        IList<TenantCulture> GetTenantCultures();
        TenantLookup GetLookupItem(int lookupId);
        TenantNotificationTemplate GetNotificationTemplate(TenantNotificationTemplateType notificationTemplateType);
        int AddTenantNotification(TenantNotification notification);
        Tenant GetTenantByYesserCode(string yesserCode);
        List<TenantNotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType);
        void UpdateNotifactionDetails(IList<TenantNotificationDetail> tenantNotificationDetail);
        IList<UserTenant> GetAllUserTenants();
        int AddEditUserTenant(UserTenant tenant);
        bool IsExistTenantUserName(string userName, int id);
        UserTenant GetUserTenantById(int id);
        void DeleteUserTenant(int id);
    }
}
