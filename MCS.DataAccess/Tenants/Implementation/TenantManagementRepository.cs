using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    #region Enums

    public enum TenantNotificationEmailSubject
    {
        TaskReminder = 1582,
        TransactionAssignment = 6,
        ResetPassword = 1643,
        NewUser = 8,
        NewTask = 2687,
        FollowUpRecieveEmail = 2690
    }

    public enum TenantNotificationTemplateType
    {
        None = 0,
        TenantAdminEmail = 7,
        TransactionAssignmentEmail = 7,//إحالة معاملة
        TransactionAssignmentDraftEmail = 445,//إحالة معاملة

        TaskReminderEmail = 446,
        NewTaskEmail = 447,
        DeleteTaskEmail = 448,
        ResendTaskEmail = 449,
        AcceptTaskEmail = 450,
        RejectTaskEmail = 451,
        ReplyTaskEmail = 452,

        AssignTransactionEmail = 453,
        RevertRejectTransactionEmail = 454,
        RevertTransactionEmail = 455,

        ElectronicCopiesEmail = 456,//وصول نسخة الكترونية من المعاملة 
        ViewedEmail = 457,//تم الاطلاع على النسخة الإلكترونية لمعاملة 

        FollowupEmail = 458,
        CancelFollowupEmail = 459,
        EndFollowupPeriodEmail = 460,
        CancelFollowupSendToSavedEmail = 461,

        AddExplanationEmail = 462,
        ReceiveReportEmail = 463,//استلام بيان التسليم

        AddDelegationEmail = 464,
        InProcessDelegationEmail = 465,
        ApprovedDelegationEmail = 466,
        RejectedDelegationEmail = 467,
        DisabledDelegationEmail = 468,
        EnableDelegationEmail = 469,

        OrgUnitEmail = 470
    }

    public enum TenantNotificationType
    {
        Email = 10
    }

    public enum TenantNotificationSource
    {
        TaskReminder = 388,
        TransactionAssignment = 389,
        ResetPassword = 390,
        NewUser = 391,
        NewTask = 392,
        DeleteTask = 421,
        ResendTask = 422,
        AcceptTask = 423,
        RejectTask = 424,
        ReplyTask = 425,
        AssignTransaction = 426,
        RevertRejectTransaction = 427,
        RevertTransaction = 428,
        ElectronicCopies = 429,
        Viewed = 430,
        Followup = 431,
        CancelFollowup = 432,
        EndFollowupPeriod = 433,
        CancelFollowupSendToSaved = 434,
        AddExplanation = 435,
        ReceiveReport = 436,
        AddDelegation = 437,
        InProcessDelegation = 438,
        ApprovedDelegation = 439,
        RejectedDelegation = 440,
        DisabledDelegation = 441,
        EnableDelegation = 442
    }

    #endregion Enums

    public class TenantManagementRepository : EFRepository<Tenant>, ITenantManagementRepository
    {
        #region Attributes

        private MasterDbContext _dbContext = null;

        #endregion Attributes

        #region Constructors

        public TenantManagementRepository(MasterDbContext context)
            : base(context)
        {
            _dbContext = context;
        }

        #endregion Constructors

        public int AddTenant(Tenant tenant)
        {
            try
            {
                _dbContext.Tenants.Add(tenant);

                _dbContext.SaveChanges();

                return tenant.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void UpdateTenant(Tenant tenant)
        {
            try
            {
                Tenant tenantOld = GetTenantById(tenant.Id);

                if (tenantOld != null)
                {
                    tenantOld.DelegatedEmail = tenant.DelegatedEmail;
                    tenantOld.DelegatedMobile = tenant.DelegatedMobile;
                    tenantOld.DelegatedUserName = tenant.DelegatedUserName;
                    tenantOld.FromDate = tenant.FromDate;
                    tenantOld.FromDateH = tenant.FromDateH;
                    tenantOld.ToDate = tenant.ToDate;
                    tenantOld.ToDateH = tenant.ToDateH;
                    tenantOld.UsersCount = tenant.UsersCount;
                    tenantOld.OrgUnitsCount = tenant.OrgUnitsCount;
                    tenantOld.HostName = tenant.HostName;
                    if (tenant.Logo != null)
                    {
                        tenantOld.Logo = tenant.Logo;
                    }
                    if (tenant.YesserCertificate != null)
                    {
                        tenantOld.YesserCertificate = tenant.YesserCertificate;
                    }

                    foreach (TenantLocalization localization in tenant.Name.Localizations)
                    {
                        TenantLocalization currentlocalization = tenantOld.Name.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            localization.LocalizationIdentifierId = currentlocalization.LocalizationIdentifierId;
                            _dbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    foreach (TenantLocalization localization in tenant.DelegatedName.Localizations)
                    {
                        TenantLocalization currentlocalization = tenantOld.DelegatedName.Localizations
                         .Where(l => l.Id == localization.Id).FirstOrDefault();

                        if (currentlocalization != null)
                        {
                            localization.LocalizationIdentifierId = currentlocalization.LocalizationIdentifierId;
                            _dbContext.Entry(currentlocalization).CurrentValues.SetValues(localization);
                        }
                    }

                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public List<TenantNotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType)
        {
            int notificationTypeId = notificationType.LookupIdentity(LookupCategory.NotificationType, string.Empty);
            return _dbContext.TenantNotificationDetails.Where(x => x.IsSent == false && x.FailureCount <= failureCount && x.TypeId == notificationTypeId)
                 .Include(a => a.Attachments).ToList();
        }

        public Tenant GetTenantById(int tenantId)
        {
            try
            {
                var result = _dbContext.Tenants.Include(a => a.UserTenants).FirstOrDefault(t => t.Id == tenantId);
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Tenant GetTenantByYesserCode(string yesserCode)
        {
            try
            {
                return FindBy(t => t.YesserCode == yesserCode);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Tenant GetTenantByHostName(string hostName)
        {
            try
            {
                IList<Tenant> tenants = (from tenant in _dbContext.Tenants
                                         where tenant.HostName == hostName
                                         select new
                                         {
                                             Id = tenant.Id,
                                             HostName = tenant.HostName,
                                             Database = tenant.DatabaseName,
                                             ToDate = tenant.ToDate,
                                             FromDate = tenant.FromDate,
                                             IsActive = tenant.IsActive,
                                         }).ToList().Select(t => new Tenant
                                         {
                                             Id = t.Id,
                                             HostName = t.HostName,
                                             DatabaseName = t.Database,
                                             ToDate = t.ToDate,
                                             FromDate = t.FromDate,
                                             IsActive = t.IsActive,
                                         }).AsQueryable().ToList();

                return tenants.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Tenant GetTenantByUserName(string userName, string cultureName)
        {
            try
            {
                var userTenant = _dbContext.UserTenants
                                           .FirstOrDefault(ut => ut.UserName.ToLower() == userName.ToLower());
                if (userTenant == null)
                {
                    return null;
                }

                return _dbContext.Tenants
                                       .Where(t => t.Id == userTenant.TenantId)
                                       .Select(t => new
                                       {
                                           t.Id,
                                           t.HostName,
                                           t.DatabaseName,
                                           t.ToDate,
                                           t.FromDate,
                                           t.IsActive,
                                           t.Logo,
                                           LocalName = t.Name.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text
                                       }).ToList().Select(t => new Tenant
                                       {

                                           Id = t.Id,
                                           HostName = t.HostName,
                                           DatabaseName = t.DatabaseName,
                                           ToDate = t.ToDate,
                                           FromDate = t.FromDate,
                                           IsActive = t.IsActive,
                                           Logo = t.Logo,
                                           LocalName = t.LocalName
                                       }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public Tenant GetTenantById(int tenantId, bool validation)
        {
            try
            {
                return GetTenantById(tenantId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Tenant> GetTenants(System.Linq.Expressions.Expression<Func<Tenant, bool>> @where, string cultureName)
        {
            try
            {
                IList<Tenant> tenants = (from tenant in _dbContext.Tenants.Where(@where).Where(t => !t.IsDeleted)
                                         select new
                                         {
                                             Id = tenant.Id,
                                             Name = tenant.Name,
                                             ConnectionString = tenant.DatabaseName,
                                             HostName = tenant.HostName,
                                             FromDate = tenant.FromDate,
                                             ToDate = tenant.ToDate,
                                             OrgUnitsCount = tenant.OrgUnitsCount,
                                             UsersCount = tenant.UsersCount,
                                             DelegatedName = tenant.DelegatedName,
                                             DelegatedUserName = tenant.DelegatedUserName,
                                             DelegatedEmail = tenant.DelegatedEmail,
                                             DelegatedMobile = tenant.DelegatedMobile,
                                             IsActive = tenant.IsActive,
                                         }).ToList().Select(t => new Tenant
                                         {
                                             Id = t.Id,
                                             DatabaseName = t.ConnectionString,
                                             HostName = t.HostName,
                                             FromDate = t.FromDate,
                                             ToDate = t.ToDate,
                                             IsActive = t.IsActive,
                                             OrgUnitsCount = t.OrgUnitsCount,
                                             UsersCount = t.UsersCount,
                                             DelegatedEmail = t.DelegatedEmail,
                                             DelegatedMobile = t.DelegatedMobile,
                                             LocalName = t.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                             LocalDelegatedName = t.DelegatedName.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                         }).AsQueryable().ToList();

                return tenants;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tenant> GetTenants(SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<Tenant> tenants = from tenant in _dbContext.Tenants.Include("Name").Include("DelegatedName")
                                             where tenant.IsDeleted == false
                                             select tenant;

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        PropertyInfo propertyInfo = typeof(Tenant).GetProperty(filter.ColumnName);

                        if (propertyInfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            tenants = SortByText(tenants, filter.Value, filter.Type, searchCriteria.CultureName);
                        }
                        else
                        {
                            tenants = WhereQuery(tenants, filter.ColumnName, filter.Value, filter.Type);
                        }
                    }
                }

                rowsCount = tenants.Count();

                //if (!searchCriteria.Ascending)
                //{
                //    tenants = tenants.OrderByDescending(t => t.Id)
                //   .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                //       .Take(searchCriteria.PageSize);
                //}
                //else
                //{
                tenants = tenants.OrderBy(t => t.Id)
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                    .Take(searchCriteria.PageSize);
                //}

                return tenants.ToList().Select(t => new Tenant
                {
                    Id = t.Id,
                    DatabaseName = t.DatabaseName,
                    HostName = t.HostName,
                    FromDate = t.FromDate,
                    ToDate = t.ToDate,
                    OrgUnitsCount = t.OrgUnitsCount,
                    UsersCount = t.UsersCount,
                    DelegatedEmail = t.DelegatedEmail,
                    DelegatedMobile = t.DelegatedMobile,
                    IsActive = t.IsActive,
                    Name = t.Name,
                    DelegatedName = t.DelegatedName,
                    LocalName = t.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                    LocalDelegatedName = t.DelegatedName.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Tenant> GetAllTenants(string cultureName)
        {
            try
            {
                IList<Tenant> tenants = (from tenant in _dbContext.Tenants
                                         where !tenant.IsDeleted
                                         select new
                                         {
                                             Id = tenant.Id,
                                             Name = tenant.Name,
                                             ConnectionString = tenant.DatabaseName,
                                             HostName = tenant.HostName,
                                             FromDate = tenant.FromDate,
                                             ToDate = tenant.ToDate,
                                             OrgUnitsCount = tenant.OrgUnitsCount,
                                             UsersCount = tenant.UsersCount,
                                             DelegatedName = tenant.DelegatedName,
                                             DelegatedUserName = tenant.DelegatedUserName,
                                             DelegatedEmail = tenant.DelegatedEmail,
                                             DelegatedMobile = tenant.DelegatedMobile,
                                             IsActive = tenant.IsActive
                                         }).ToList().Select(t => new Tenant
                                         {
                                             Id = t.Id,
                                             DatabaseName = t.ConnectionString,
                                             HostName = t.HostName,
                                             FromDate = t.FromDate,
                                             ToDate = t.ToDate,
                                             IsActive = t.IsActive,
                                             OrgUnitsCount = t.OrgUnitsCount,
                                             UsersCount = t.UsersCount,
                                             DelegatedEmail = t.DelegatedEmail,
                                             DelegatedMobile = t.DelegatedMobile,
                                             LocalName = t.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                                             LocalDelegatedName = t.DelegatedName.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                                         }).AsQueryable().ToList();

                return tenants;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TenantCulture GetTenantCulture(int cultureId)
        {
            try
            {
                return _dbContext.Cultures.Find(cultureId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TenantCulture> GetTenantCultures()
        {
            try
            {
                return _dbContext.Cultures.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TenantLookup GetLookupItem(int lookupId)
        {
            try
            {
                return _dbContext.TenantLookups.Where(l => l.Id == lookupId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TenantNotificationTemplate GetNotificationTemplate(TenantNotificationTemplateType notificationTemplateType)
        {
            try
            {
                return _dbContext.NotificationTemplates.Where(n => n.TypeId == (int)notificationTemplateType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int AddTenantNotification(TenantNotification notification)
        {
            try
            {
                _dbContext.Notifications.Add(notification);
                _dbContext.SaveChanges();

                return notification.Id;
            }

            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Tenant> SortByText(IQueryable<Tenant> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from priority in source.Where(t => t.Name.Localizations.FirstOrDefault().Text.Contains(textValue))
                            select priority);
                case FilterType.EndsWidth:
                    return (from priority in source.Where(t => t.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue))
                            select priority);
                case FilterType.StartsWith:
                    return (from priority in source.Where(t => t.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue))
                            select priority);
                case FilterType.Equals:
                    return (from priority in source.Where(t => t.Name.Localizations.Where(l => l.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue))
                            select priority);
            }

            return source;
        }

        public void UpdateNotifactionDetails(IList<TenantNotificationDetail> tenantNotificationDetail)
        {
            foreach (var item in tenantNotificationDetail)
            {
                var row = _dbContext.TenantNotificationDetails.Single(x => x.Id == item.Id);
                row.IsSent = item.IsSent;
                row.FailureCount = item.FailureCount;
            }

            _dbContext.SaveChanges();
        }

        public int AddEditUserTenant(UserTenant userTenant)
        {
            try
            {
                if (userTenant.Id == 0)
                {
                    _dbContext.UserTenants.Add(userTenant);
                }
                else
                {
                    var tenantOld = _dbContext.UserTenants.FirstOrDefault(a => a.Id == userTenant.Id);
                    tenantOld.UserName = userTenant.UserName;
                    tenantOld.TenantId = userTenant.TenantId;
                }

                _dbContext.SaveChanges();
                return userTenant.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<UserTenant> GetAllUserTenants()
        {
            try
            {
                IList<UserTenant> userTenants = (from tenant in _dbContext.UserTenants
                                                 select new
                                                 {
                                                     tenant.Id,
                                                     tenant.UserName,
                                                     tenant.Tenant
                                                 }).ToList().Select(t => new UserTenant
                                                 {
                                                     Id = t.Id,
                                                     UserName = t.UserName,
                                                     Tenant = t.Tenant,
                                                 }).AsQueryable().ToList();

                return userTenants;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool IsExistTenantUserName(string userName, int id)
        {
            bool isExist;
            try
            {
                var userTenant = (from item in _dbContext.UserTenants
                                  where item.UserName == userName && item.Id != id
                                  select new
                                  {
                                      item.TenantId
                                  }).ToList().Select(t => new UserTenant
                                  {
                                      TenantId = t.TenantId
                                  }).AsQueryable().FirstOrDefault();

                isExist = userTenant != null;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
            return isExist;
        }

        public UserTenant GetUserTenantById(int id)
        {
            try
            {
                var result = _dbContext.UserTenants.FirstOrDefault(a => a.Id == id);
                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteUserTenant(int id)
        {
            try
            {
                var result = _dbContext.UserTenants.FirstOrDefault(a => a.Id == id);
                _dbContext.UserTenants.Remove(result);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
