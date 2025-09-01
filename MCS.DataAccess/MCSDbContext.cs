using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using MCS.Framework;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess.Mappings;
using MCS.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Core.Metadata.Edm;
using Audit.Core;
using MCS.Framework.Web;
using MCS.Framework.Security;
using System.Linq;
using Audit.EntityFramework;
using System.Web;
using MCS.Domain.IC;

namespace MCS.DataAccess
{
    [AuditDbContext(Mode = AuditOptionMode.OptOut, IncludeEntityObjects = false,
        AuditEventType = "{database}_{context}")]

    [DbConfigurationType(typeof(CodeConfig))]
    public class MCSDbContext : DbContextBase, IDbModelCacheKeyProvider
    {
        static MCSDbContext()
        {
            //if (!SystemConfigurations.MultiTenantCreateDatabaseEnabled)
            //{
            Database.SetInitializer<MCSDbContext>(new NullDatabaseInitializer<MCSDbContext>());
            //}
        }
        public MCSDbContext() : this(tenantId: null)
        {
        }
        public MCSDbContext(int? tenantId) : base("name=eMorasalat")
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                ITenant tenantInfo = null;

                if (tenantId != -1 && tenantId.HasValue)
                {
                    ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                    Tenant tenant = tenantManagementRepository.GetTenantById(tenantId.Value, true);

                    tenantInfo = new TenantInfo
                    {
                        Id = tenant.Id,
                        HostName = tenant.HostName,
                        DatabaseName = tenant.DatabaseName,
                        ECMProfileId = tenant.ECMProfileId,
                        ECMCategoryId = tenant.ECMCategoryId
                    };
                }
                else
                {
                    tenantInfo = new TenantInfo
                    {
                        DatabaseName = TenantHelper.GetTenantDatabaseNameFromHeader()
                    };
                }

                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    IConnectionStringBuilder connectionStringBuilder = IoC.Resolve<IConnectionStringBuilder>();
                    string updateDatabaseConnection = connectionStringBuilder.UpdateDatabaseName(Database.Connection.ConnectionString, tenantInfo.DatabaseName);
                    Database.Connection.ConnectionString = updateDatabaseConnection;
                }
                else
                {
                    IOracleConnectionStringBuilder oracleConnectionStringBuilder = IoC.Resolve<IOracleConnectionStringBuilder>();
                    string updateDatabaseConnection = oracleConnectionStringBuilder.UpdateDatabaseName(Database.Connection.ConnectionString, tenantInfo.DatabaseName);
                    Database.Connection.ConnectionString = updateDatabaseConnection;
                }
            }
            if (SystemConfigurations.MultiTenantCreateDatabaseEnabled)
            {
                //Disable the following for max performance when doing READONLY operations
                //Whenever you need to add/edit records, enable ProxyCreationEnabled and AutoDetectChangesEnabled on that level
                base.Configuration.LazyLoadingEnabled = false;
                base.Configuration.ProxyCreationEnabled = false;
                base.Configuration.AutoDetectChangesEnabled = false;
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["IsAuditingEnabled"]))
            {
                bool isAuditingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAuditingEnabled"]);
                if (isAuditingEnabled)
                {
                    AuditingInterceptor auditingInterceptor = new AuditingInterceptor();
                    Interceptors.Add(auditingInterceptor);
                }
            }
        }
        public MCSDbContext(string connection) : base(GetConnectionString(connection))
        {

            if (SystemConfigurations.MultiTenantCreateDatabaseEnabled)
            {
                Configuration.LazyLoadingEnabled = false;
                Configuration.ProxyCreationEnabled = false;
                Configuration.AutoDetectChangesEnabled = false;
            }
        }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
        public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<DocumentAttribute> DocumentAttributes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        //public DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<TaskReminder> TaskReminders { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<OrgUnitLink> OrgUnitLinks { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Name> Names { get; set; }
        public DbSet<TransactionHistory> TransactionHistory { get; set; }
        public DbSet<TransactionCopy> TransactionCopies { get; set; }
        public DbSet<TransactionExternalCopy> TransactionExternalCopies { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<TransactionAssignment> TransactionAssignments { get; set; }
        public DbSet<TransactionAssignee> TransactionAssignees { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Tray> Trays { get; set; }
        public DbSet<Domain.Action> Actions { get; set; }
        public DbSet<Counter> Counters { get; set; }
        public DbSet<CounterDetail> CounterDetails { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentInfo> DocumentsInfo { get; set; }
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<BarcodeDesign> BarcodeDesigns { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Escalation> Escalations { get; set; }
        public DbSet<PriorityException> PriorityExceptions { get; set; }
        public DbSet<LookupLocalization> LookupLocalizations { get; set; }
        public DbSet<TransactionLink> TransactionLinks { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<LetterType> LetterTypes { get; set; }
        public DbSet<AttachmentType> AttachmentTypes { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<ExternalParty> ExternalParties { get; set; }
        public DbSet<ExternalPartyAttachment> ExternalPartyAttachments { get; set; }
        public DbSet<ExternalPartyManager> ExternalPartyManagers { get; set; }
        public DbSet<UserCategoryTray> UserCategoryTrays { get; set; }
        public DbSet<MCS.Domain.Audit> Audits { get; set; }
        public DbSet<AuditDetail> AuditDetails { get; set; }
        public DbSet<AssignmentPaperBeneficiary> AssignmentPaperBeneficies { get; set; }
        public DbSet<AssignmentPaper> AssignmentPapers { get; set; }
        public DbSet<AssignmentPaperAction> AssignmentPaperActions { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<TransactionAssignmentHistory> TransactionAssignmentHistories { get; set; }
        public DbSet<TransactionIndexLog> TransactionIndexes { get; set; }
        public DbSet<TaskWorkflow> TaskWorkflows { get; set; }
        public DbSet<AssignmentGroup> AssignmentGroups { get; set; }
        public DbSet<AssignmentGroupDetail> AssignmentGroupDetails { get; set; }
        public DbSet<FormDepartment> FormDepartments { get; set; }
        public DbSet<Explanation> Explanations { get; set; }
        public DbSet<TransactionName> TransactionNames { get; set; }
        public DbSet<TransactionDeliveryReport> TransactionDeliveryReports { get; set; }
        public DbSet<SignedDeliveryReport> SignedDeliveryReports { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<SubjectClassification> SubjectClassifications { get; set; }
        public DbSet<SuggestedTopic> SuggestedTopics { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<SubjectOrgUnit> SubjectOrgUnits { get; set; }
        public DbSet<TransactionSubjectClassification> TransactionSubjectClassifications { get; set; }
        public DbSet<UserPreference> UserPreference { get; set; }
        public DbSet<UserDelegation> UserDelegations { get; set; }
        public DbSet<DocProviders> DocProviders { get; set; }
        public DbSet<LocalizationIdentifier> LocalizationIdentifiers { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<HubTransaction> HubTransactions { get; set; }
        public DbSet<HubRecord> HubRecords { get; set; }
        public DbSet<HubRQUID> HubRQUIDs { get; set; }
        public DbSet<YesserMapping> YesserMappings { get; set; }
        public DbSet<YesserNewEntites> YesserNewEntites { get; set; }
        public DbSet<TransactionEntityDetails> TransactionEntityDetails { get; set; }
        public DbSet<HubAttachment> HubAttachments { get; set; }
        public DbSet<HubRelatedPerson> HubRelatedPersons { get; set; }
        public DbSet<DistributionList> distributionLists { get; set; }
        public DbSet<DistributionListDetails> DistributionListDetails { get; set; }
        public DbSet<TasksAttachments> TasksAttachments { get; set; }
        public DbSet<TransactionFollowUp> TransactionFollowUps { get; set; }
        public DbSet<FollowUpDetails> FollowUpDetails { get; set; }
        public DbSet<TransactionReservation> TransactionReservations { get; set; }
        public DbSet<TransactionPath> TransactionPaths { get; set; }
        public DbSet<TransactionPathDetails> TransactionPathDetails { get; set; }
        public DbSet<ChatClient> ChatClients { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<ChatMessagesStatus> MessagesStatus { get; set; }
        public DbSet<ChatRoomUser> ChatRoomUsers { get; set; }
        public DbSet<ChatRoomOwner> ChatRoomOwners { get; set; }
        public DbSet<ChatRoomAllowedUser> ChatRoomAllowedUsers { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Reporter> Reporters { get; set; }
        public DbSet<NotificationDetail> NotificationDetails { get; set; }
        public DbSet<UserMobile> UserMobiles { get; set; }
        public DbSet<AttachmentExtension> AttachmentExtensions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<SystemDefaultValues> SystemDefaultValues { get; set; }
        public DbSet<UserPreferenceFollowup> UserPreferenceFollowups { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<AssignmentPaperGroup> AssignmentPaperGroups { get; set; }
        public DbSet<AllowedAssignment> AllowedAssignments { get; set; }
        public DbSet<UserPendingGroup> UserPendingGroups { get; set; }
        public DbSet<FollowUpPriorityType> FollowUpPriorityTypes { get; set; }
        public DbSet<FollowUpSource> FollowUpSources { get; set; }
        public DbSet<FollowUpProccess> FollowUpProccess { get; set; }
        public DbSet<FollowUpMethod> FollowUpMethods { get; set; }
        public DbSet<FollowUpAuditTrail> FollowUpAuditTrails { get; set; }
        public DbSet<TransactionElcOutBound> TransactionElcOutBounds { get; set; }
        #region IC
        public DbSet<IC_SUBJECT> IC_SUBJECTS { get; set; }
        public DbSet<IC_SUBJECTS_TRANSACTION> IC_SUBJECTS_TRANSACTIONS { get; set; }
        public DbSet<IC_CLASSIFICATION> IC_CLASSIFICATIONS { get; set; }
        public DbSet<IC_DOC_STATUS> IC_DOC_STATUS { get; set; }
        public DbSet<IC_DOCS> IC_DOCS { get; set; }
        public DbSet<IC_FILE> IC_FILE { get; set; }
        public DbSet<IC_FILE_ALLOCATION> IC_FILE_ALLOCATION { get; set; }
        public DbSet<IC_FILE_COUNTER> IC_FILE_COUNTER { get; set; }
        public DbSet<IC_FILE_PARTS> IC_FILE_PARTS { get; set; }
        public DbSet<IC_INDEX> IC_INDEX { get; set; }
        public DbSet<IC_INDEX_CLASSIFICATION> IC_INDEX_CLASSIFICATION { get; set; }
        public DbSet<IC_OFFICE> IC_OFFICE { get; set; }


        #endregion


        public DbSet<SpecificLevel> SpecificLevels { get; set; }
        public DbSet<ReleaseNote> ReleaseNotes { get; set; }
        public DbSet<ReleaseNotesUser> ReleaseNotesUsers { get; set; }
        public DbSet<ConfidentialityAcknowledgment> ConfidentialityAcknowledgments { get; set; }
        public DbSet<TransactionConfidAcknowledged> TransactionConfidAcknowledgeds { get; set; }
        //public DbSet<TransactionOldDocument> TransactionOldDocuments { get; set; } 
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public DbSet<SurveyNote> SurveyNotes { get; set; }
        public DbSet<WordAddInTemp> WordAddInTemps { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ApiAuditLog> ApiAuditLogs { get; set; }

        public DbSet<TransactionSpecialAuthorize> TransactionSpecialAuthorizes { get; set; }
        public DbSet<OnlineUser> OnlineUsers { get; set; }
        public DbSet<OrgunitSap> OrgunitSaps { get; set; }

        public DbSet<TransactionEncryptionCode> TransactionEncryptionCodes { get; set; }
        public DbSet<SavedTransactionAssignment> SavedTransactionAssignments { get; set; }


        public string CacheKey
        {
            get
            {
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Database.Connection.ConnectionString);
                    return sqlConnectionStringBuilder.InitialCatalog + "_" + sqlConnectionStringBuilder.DataSource;
                }
                else
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Database.Connection.ConnectionString);
                    return sqlConnectionStringBuilder.UserID + "_" + sqlConnectionStringBuilder.DataSource;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                if (!SystemConfigurations.IsOracleMigrationEnabled)
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Database.Connection.ConnectionString);
                    modelBuilder.HasDefaultSchema(SystemConfigurations.SchemaNameDatabaseType);
                }
                else
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(Database.Connection.ConnectionString);
                    modelBuilder.HasDefaultSchema(sqlConnectionStringBuilder.UserID);
                }
            }
            else
            {
                modelBuilder.HasDefaultSchema(SystemConfigurations.SchemaNameDatabaseType);
            }

            //Auto migrate Database to latest version using Migrations
            if (SystemConfigurations.MultiTenantCreateDatabaseEnabled)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MCSDbContext, Migrations.Configuration>());
            }
            if (SystemConfigurations.IsOracleMigrationEnabled)
            {
                modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(1000));
            }

            modelBuilder.Configurations.Add(new FormMapping());
            modelBuilder.Configurations.Add(new UserProfileMapping());
            modelBuilder.Configurations.Add(new UserPermissionMapping());
            modelBuilder.Configurations.Add(new OrgUnitMapping());
            modelBuilder.Configurations.Add(new OrgUnitLinkMapping());
            modelBuilder.Configurations.Add(new ExternalPartyMapping());
            modelBuilder.Configurations.Add(new ExternalPartyManagerMapping());
            modelBuilder.Configurations.Add(new DocumentInfoMapping());
            modelBuilder.Configurations.Add(new AttachmentMapping());
            modelBuilder.Configurations.Add(new GroupMapping());
            modelBuilder.Configurations.Add(new PermissionMapping());
            modelBuilder.Configurations.Add(new LookupMapping());
            modelBuilder.Configurations.Add(new ActionMapping());
            modelBuilder.Configurations.Add(new AttachmentTypeMapping());
            modelBuilder.Configurations.Add(new LetterTypeMapping());
            modelBuilder.Configurations.Add(new LinkMapping());
            modelBuilder.Configurations.Add(new PriorityMapping());
            modelBuilder.Configurations.Add(new TransactionTypeMapping());
            modelBuilder.Configurations.Add(new TransactionMapping());
            modelBuilder.Configurations.Add(new TransactionLinkMapping());
            modelBuilder.Configurations.Add(new TrayMapping());
            modelBuilder.Configurations.Add(new UserCategoryMapping());
            modelBuilder.Configurations.Add(new AssignmentGroupMapping());
            modelBuilder.Configurations.Add(new ExplanationMapping());
            modelBuilder.Configurations.Add(new SuggestedTopicMapping());
            modelBuilder.Configurations.Add(new SubjectClassificationMapping());
            modelBuilder.Configurations.Add(new UserPreferenceMapping());
            modelBuilder.Configurations.Add(new UserDelegationMapping());
            modelBuilder.Configurations.Add(new LocalizationMapping());
            modelBuilder.Configurations.Add(new CultureMapper());
            modelBuilder.Configurations.Add(new BarcodeDesignMapping());
            modelBuilder.Configurations.Add(new TransactionIndexLogMapping());
            modelBuilder.Configurations.Add(new TransactionHistoryMapping());
            modelBuilder.Configurations.Add(new TransactionDeliveryReportMapping());
            modelBuilder.Configurations.Add(new TransactionCopyMapping());
            modelBuilder.Configurations.Add(new TransactionAssignmentHistoryMapping());
            modelBuilder.Configurations.Add(new TransactionAssignmentMapping());
            modelBuilder.Configurations.Add(new TaskReminderMapping());
            modelBuilder.Configurations.Add(new TaskHistoryMapping());
            modelBuilder.Configurations.Add(new TaskMapping());
            modelBuilder.Configurations.Add(new NameMapping());
            modelBuilder.Configurations.Add(new NotificationMapping());
            modelBuilder.Configurations.Add(new NotificationAttachmentMapping());
            modelBuilder.Configurations.Add(new NotificationDetailMapping());
            //modelBuilder.Configurations.Add(new NotificationTemplateMapping());
            modelBuilder.Configurations.Add(new CollaborationMapping());
            modelBuilder.Configurations.Add(new AuditMapping());
            modelBuilder.Configurations.Add(new AuditDetailMapping());
            modelBuilder.Configurations.Add(new DocumentAttributeMapping());
            modelBuilder.Configurations.Add(new DocProviderMapping());
            modelBuilder.Configurations.Add(new AspNetRoleMapping());
            modelBuilder.Configurations.Add(new AspNetUserLoginMapping());
            modelBuilder.Configurations.Add(new AspNetUserClaimMapping());
            modelBuilder.Configurations.Add(new AspNetUserMapping());
            modelBuilder.Configurations.Add(new ResourceMapping());
            modelBuilder.Configurations.Add(new TransactionEntityDetailMapping());
            modelBuilder.Configurations.Add(new HubTransactionMapping());
            modelBuilder.Configurations.Add(new HubAttachmentMapping());
            modelBuilder.Configurations.Add(new DistributionListMapping());
            modelBuilder.Configurations.Add(new TaskAttachmentsMapping());
            modelBuilder.Configurations.Add(new TransactionReservationMapping());
            modelBuilder.Configurations.Add(new UserMobileMapping());
            modelBuilder.Configurations.Add(new ExternalPartyAttachmentMapping());
            modelBuilder.Configurations.Add(new PriorityExceptionMapping());
            modelBuilder.Configurations.Add(new EscalationMapping());
            modelBuilder.Configurations.Add(new ChatMessageMapping());
            modelBuilder.Configurations.Add(new ChatRoomMapping());
            modelBuilder.Configurations.Add(new IC_SUBJECTMapping());
            modelBuilder.Conventions.Add<CapitalizeColumnName>();
            modelBuilder.Conventions.Add<CapitalizeTableName>();
            modelBuilder.Conventions.Add<SetTableNameLenght>();
            modelBuilder.Configurations.Add(new AllowedAssignmentMapping());




            Audit.Core.Configuration.Setup()
        .UseEntityFramework(x => x
       .AuditTypeMapper(t => typeof(AuditLog))
       .AuditEntityAction<AuditLog>((ev, entry, entity) =>
       {
           entity.GuidId = UserContext.LoggedInUser != null ? UserContext.LoggedInUser.RequestId : "MobileAPI";
           entity.EntityType = entry.EntityType.Name;
           entity.AuditData = entry.ToJson();
           entity.AuditDate = DateTime.Now;
           entity.AuditUser = UserContext.LoggedInUser != null ? UserContext.LoggedInUser.Id : 0;
           entity.TablePk = entry.PrimaryKey.First().Value.ToString();
           entity.AuditAction = entry.Action;
           entity.TableName = entry.Table;
       })
          .IgnoreMatchedProperties(true));


            base.OnModelCreating(modelBuilder);
        }
        public class CapitalizeColumnName : IStoreModelConvention<EdmProperty>
        {
            public void Apply(EdmProperty item, DbModel model)
            {
                item.Name = item.Name.ToUpper();
            }
        }
        public class SetTableNameLenght : IStoreModelConvention<EntitySet>
        {
            public void Apply(EntitySet item, DbModel model)
            {
                if (item.Table.Length > 30)
                {
                    item.Table = item.Table.Substring(0, 30);
                }
            }
        }
        public class CapitalizeTableName : IStoreModelConvention<EntitySet>
        {
            public void Apply(EntitySet item, DbModel model)
            {
                item.Table = item.Table.ToUpper();
            }
        }
        public static string GetConnectionString(string connection)
        {
            if (!string.IsNullOrEmpty(connection))
            {
                return connection;
            }
            string connectionString = ConfigurationManager.ConnectionStrings["eMorasalat"].ConnectionString;
            if (SystemConfigurations.IsOracleMigrationEnabled)
                connectionString = ConfigurationManager.ConnectionStrings["eMorasalat"].ConnectionString;
            return connectionString;
        }
    }
}
