using System.Collections.Generic;

namespace MCS.Common
{
    public class Constants
    {
        public static class GeneralSettings
        {
            public const string SupportEmail = "SupportEmail";
            public const string NotifyEmployeeBeforeTaskExpiry = "NotifyEmployeeBeforeTaskExpiry";
            public const string NotifyEmployeeBeforeFollowUpExpiry = "NotifyEmployeeBeforeFollowUpExpiry";
            public const string MaxOutboundNumberCanBooked = "MaxOutboundNumberCanBooked";
            public const string MaxInboundNumberCanBooked = "MaxInboundNumberCanBooked";
            public const string MaxRequestSize = "MaxRequestSize";
            public const string DefaultRole = "DefaultRole";
            public const string SelectAction = "SelectAction";
            public const string FirstOrgUnitFoldDown = "FirstOrgUnitFoldDown";
            public const string FirstUserFoldDown = "FirstUserFoldDown";
            public const string SecondOrgUnitFoldDown = "SecondOrgUnitFoldDown";
            public const string SecondUserFoldDown = "SecondUserFoldDown";
            public const string ThirdOrgUnitFoldDown = "ThirdOrgUnitFoldDown";
            public const string ThirdUserFoldDown = "ThirdUserFoldDown";
            public const string FortOrgUnitFoldDown = "FortOrgUnitFoldDown";
            public const string FortUserFoldDown = "FortUserFoldDown";
            public const string TransactionHashedCode = "TransactionHashedCode";

        }
        public static class TraysSettings
        {
            public const string InMyTransactionTray = "MaximumNumberOfRecordsThatWillAppearPerPageInMyTransactionTray";
            public const string InMyOrgUnitTray = "MaximumNumberOfRecordsThatWillAppearPerPageInMyOrgUnitTray";
            public const string InExCopiesTray = "MaximumNumberOfRecordsThatWillAppearPerPageInExCopiesTray";
            public const string InTasksTray = "MaximumNumberOfRecordsThatWillAppearPerPageInTasksTray";
            public const string InCompleteTransactionsTray = "MaximumNumberOfRecordsThatWillAppearPerPageInCompleteTransactionsTray";
            public const string InOutboundTray = "MaximumNumberOfRecordsThatWillAppearPerPageInOutboundTray";
            public const string InSentTray = "MaximumNumberOfRecordsThatWillAppearPerPageInSentTray";
            public const string InFollowUpTray = "MaximumNumberOfRecordsThatWillAppearPerPageInFollowUpTray";
            public const string InCopiesTray = "MaximumNumberOfRecordsThatWillAppearPerPageInCopiesTray";
            public const string InManagerTray = "MaximumNumberOfRecordsThatWillAppearPerPageInManagerTray";
            public const string InMyArchivesTray = "MaximumNumberOfRecordsThatWillAppearPerPageInMyArchivesTray";

        }
        public static class CounterSetting
        {
            public const string TheInitialValueOfOutboundNumber = "TheInitialValueOfOutboundNumber";
            public const string TheInitialValueOfOutboundDraftNumber = "TheInitialValueOfOutboundDraftNumber";
            public const string TheInitialValueOfInboundNumber = "TheInitialValueOfInboundNumber";
            public const string TheInitialValueOfInternalNumber = "TheInitialValueOfInternalNumber";
        }
        public static class DateAndNumbersSettings
        {
            public const string DateType = "DateType";
            public const string DateFormat = "DateFormat";
            public const string NumberFormat = "NumberFormat";
        }
        public static class SearchSettings
        {
            public const string MaximumNumber = "MaximumNumberOfRecordsThatWillAppearPerPageInSearchResult";
        }
        public static class SMSSettings
        {
            public const string SMSService = "SMSService";
        }
        public static class EmailSettings
        {
            public const string EmailService = "EmailService";
        }
        public static class SmartPhoneSettings
        {
            public const string SmartPhoneDomainURL = "SmartPhoneDomainURL";
        }
        public static class Languages
        {
            public const string Arabic = "ar";
            public const string English = "en";
        }
        public static class AgencySettings
        {
            public const string AgencyName = "AgencyName";
            public const string AgencyNumber = "AgencyNumber";
            public const string Logo = "Logo";
        }
        public static class VersionSettings
        {
            public const string VersionName = "VersionName";
            public const string VersionNumber = "VersionNumber";
            public const string VersionReleaseDate = "VersionReleaseDate";
            public const string VersionComments = "VersionComments";
        }
        public static class NotificationIcons
        {
            public static readonly Dictionary<string, string> NotificationDetails = new Dictionary<string, string>
            {
                    { "AssignTransactionWeb", "<div class='mr-4 my-auto'><i class='icon-assignment-delivered notification-icon'></i></div>" },
                    { "NewTaskWeb","<div class='mr-4 my-auto'><i class='icon-assignment-new notification-icon'></i></div>" },
                    { "RevertTransactionWeb","<div class='mr-4 my-auto'><i class='icon-assignment-return notification-icon'></i></div>"}
            };
            public static readonly Dictionary<string, string> NotificationHeader = new Dictionary<string, string>
            {
                    { "AssignTransactionWeb", "icon-assignment-delivered" },
            };
        }
        public const string CultureNameKey = "__CultureName";
        public const string LoggedInUserKey = "__LoggedInUser";
        public const string CompanyName = "CompanyName-";
        public const string LeftDirection = "ltr";
        public const string RightDirection = "rtl";
        public const string UserHostIPAddressKey = "__UserHostIPAddress";
        public const string UserLoggingInfo = "__UserLoggingInfo";
        public const string HostName = "HostName";
        public const string SubDomainName = "SubDomainName";
        public const string TenantId = "TenantId";
        public const string TenantKey = "__TenantInfo";
        public const string ECMProfileId = "__ECMProfileId";
        public const string ECMCategoryId = "__ECMCategoryId";
        public const string TenantDatabaseName = "__TenantDatabaseName";
        public const string DocViewerSessionKey = "SessionKey";
        public const string WithWatermarkKey = "WithWatermark";
        public const string WatermarkTextKey = "WatermarkText";
        public const string UserIdentity = "UserIdentity";
        public const string UserId = "UserId";
        public const string SettingDate = "DateType";
        public const string IsVIPUser = "IsVIPUser";
        public const string DefaultDisplay = "DefaultDisplay";
        public const string DefaultAssignmentPaper = "DefaultAssignmentPaper";
        public const string IsManager = "IsManager";
        public const string ExplanationFile = "ExplanationFile";


        public sealed class SettingsConstants
        {
            public const string Logo = "Logo";
            public const string LogoHeight = "LogoHeight";
            public const string LogoWidth = "LogoWidth";
            public const string LogoSize = "LogoSize";

        }

    }
}
