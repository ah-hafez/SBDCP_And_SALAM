using System;
using System.Configuration;
using MCS.Common.Utility;

namespace MCS.Common
{
    public class SystemConfigurations
    {

        public static int TaskLimitationLevel
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TaskLimitationLevel"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["TaskLimitationLevel"]);
                }

                throw new Exception("TaskLimitationLevel not configured in the web config file");
            }
        }

        public static int OrgStructureLimitationLevel
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ParentUnitChildrenCapacity"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ParentUnitChildrenCapacity"]);
                }

                throw new Exception("ParentUnitChildrenCapacity not configured in the web config file");
            }
        }

        public static string WebApiUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiUrl"]))
                {
                    return ConfigurationManager.AppSettings["WebApiUrl"];
                }

                throw new Exception("WebApiUrl not configured in the web config file");
            }
        }

        public static string DocsPath
        {
            get
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DocsPath"].ToString()))
                {
                    return ConfigurationManager.AppSettings["DocsPath"].ToString();
                }

                throw new Exception("DocsPath not configured in the web config file");
            }
        }
        public static string officeOnlineHost
        {
            get
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ServerOfficeOnlineHost"].ToString()))
                {
                    return ConfigurationManager.AppSettings["ServerOfficeOnlineHost"].ToString();
                }

                throw new Exception("officeOnlineHost not configured in the web config file");
            }
        }
        public static string TenantsWebApiUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TenantsWebApiUrl"]))
                {
                    return ConfigurationManager.AppSettings["TenantsWebApiUrl"];
                }

                throw new Exception("TenantsWebApiUrl not configured in the web config file");
            }
        }

        public static bool IsSolrIndexingEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSolrIndexing"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSolrIndexing"].ToLower());
                }

                throw new Exception("EnableSolrIndexing not configured in the web config file");
            }
        }

        public static bool IsCachingEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableCaching"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCaching"].ToLower());
                }

                throw new Exception("EnableCaching not configured in the web config file");
            }
        }

        public static bool IsSMSEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSMS"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSMS"].ToLower());
                }

                throw new Exception("EnableSMS not configured in the web config file");
            }
        }

        public static string ReportServerUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerUrl"]))
                {
                    return ConfigurationManager.AppSettings["ReportServerUrl"];
                }

                throw new Exception("ReportServerUrl not configured in the web config file");
            }
        }

        public static string ReportPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportPath"]))
                {
                    return ConfigurationManager.AppSettings["ReportPath"];
                }

                throw new Exception("ReportPath not configured in the web config file");
            }
        }

        public static string ReportServerUserName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerUserName"]))
                {
                    return ConfigurationManager.AppSettings["ReportServerUserName"];
                }

                throw new Exception("ReportServerUserName not configured in the web config file");
            }
        }

        public static string ReportServerPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerPassword"]))
                {
                    return ConfigurationManager.AppSettings["ReportServerPassword"];
                }

                throw new Exception("ReportServerUserName not configured in the web config file");
            }
        }

        public static string ReportServerDomain
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerDomain"]))
                {
                    return ConfigurationManager.AppSettings["ReportServerDomain"];
                }

                throw new Exception("ReportServerDomain not configured in the web config file");
            }
        }

        public static string LDAPServerName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPServerName"]))
                {
                    return ConfigurationManager.AppSettings["LDAPServerName"];
                }

                throw new Exception("LDAPServerName not configured in the web config file");
            }
        }

        public static string LDAPDomainName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPDomainName"]))
                {
                    return ConfigurationManager.AppSettings["LDAPDomainName"];
                }

                throw new Exception("LDAPDomainName not configured in the web config file");
            }
        }

        public static string LDAPUserName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPUserName"]))
                {
                    return ConfigurationManager.AppSettings["LDAPUserName"];
                }

                throw new Exception("LDAPUserName not configured in the web config file");
            }
        }

        public static string LDAPPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LDAPPassword"]))
                {
                    return ConfigurationManager.AppSettings["LDAPPassword"];
                }

                throw new Exception("LDAPPassword not configured in the web config file");
            }
        }

        public static string NotificationStayTime
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationStayTime"]))
                {
                    return ConfigurationManager.AppSettings["NotificationStayTime"];
                }

                throw new Exception("NotificationStayTime not configured in the web config file");
            }
        }
        public static string NotificationBaseUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NotificationBaseUrl"]))
                {
                    return ConfigurationManager.AppSettings["NotificationBaseUrl"];
                }

                throw new Exception("Notification Base Url is not configured in the web config file");
            }
        }

        public static string ResetPasswordUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ResetPasswordUrl"]))
                {
                    return ConfigurationManager.AppSettings["ResetPasswordUrl"];
                }

                throw new Exception("ResetPasswordUrl not configured in the web config file");
            }
        }

        public static string BarcodeInchMinHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeInchMinHeight"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeInchMinHeight"];
                }

                throw new Exception("BarcodeInchMinHeight not configured in the web config file");
            }
        }

        public static string BarcodeCmMinHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeCmMinHeight"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeCmMinHeight"];
                }

                throw new Exception("BarcodeCmMinHeight not configured in the web config file");
            }
        }

        public static string BarcodeInchMinWidth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeInchMinWidth"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeInchMinWidth"];
                }

                throw new Exception("BarcodeInchMinWidth not configured in the web config file");
            }
        }

        public static string BarcodeCmMinWidth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeCmMinWidth"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeCmMinWidth"];
                }

                throw new Exception("BarcodeCmMinWidth not configured in the web config file");
            }
        }

        public static string BarcodeInchMaxHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeInchMaxHeight"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeInchMaxHeight"];
                }

                throw new Exception("BarcodeInchMaxHeight not configured in the web config file");
            }
        }

        public static string BarcodeCmMaxHeight
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeCmMaxHeight"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeCmMaxHeight"];
                }

                throw new Exception("BarcodeCmMaxHeight not configured in the web config file");
            }
        }

        public static string BarcodeInchMaxWidth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeInchMaxWidth"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeInchMaxWidth"];
                }

                throw new Exception("BarcodeInchMaxWidth not configured in the web config file");
            }
        }

        public static string BarcodeCmMaxWidth
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeCmMaxWidth"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeCmMaxWidth"];
                }

                throw new Exception("BarcodeCmMaxWidth not configured in the web config file");
            }
        }

        public static string ReportLogoPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportLogoPath"]))
                {
                    return ConfigurationManager.AppSettings["ReportLogoPath"];
                }

                throw new Exception("ReportLogoPath not configured in the web config file");
            }
        }

        public static bool MultiTenantEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["MultiTenantEnabled"].TryGetBoolValue();
            }
        }

        public static string ExternalCopiesAttachmentPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExternalCopiesAttachmentPath"]))
                {
                    return ConfigurationManager.AppSettings["ExternalCopiesAttachmentPath"];
                }

                throw new Exception("ExternalCopiesAttachmentPath not configured in the web config file");
            }
        }
        public static string validFileExtentions
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["validFileExtentions"]))
                {
                    return ConfigurationManager.AppSettings["validFileExtentions"];
                }

                throw new Exception("file extention not valid");
            }
        }
        public static string TasksAttachmentPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TasksAttachmentPath"]))
                {
                    return ConfigurationManager.AppSettings["TasksAttachmentPath"];
                }

                throw new Exception("TasksAttachmentPath not configured in the web config file");
            }
        }
        public static string BarcodeImgPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BarcodeImgPath"]))
                {
                    return ConfigurationManager.AppSettings["BarcodeImgPath"];
                }

                throw new Exception("BarcodeImgPath not configured in the web config file");
            }
        }

        public static string MultiTenantAppPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantAppPath"]))
                {
                    return ConfigurationManager.AppSettings["MultiTenantAppPath"];
                }

                throw new Exception("MultiTenantAppPath not configured in the web config file");
            }
        }

        public static string ManagedRuntimeVersion
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ManagedRuntimeVersion"]))
                {
                    return ConfigurationManager.AppSettings["ManagedRuntimeVersion"];
                }

                throw new Exception("ManagedRuntimeVersion not configured in the web config file");
            }
        }

        public static string MultiTenantResetPage
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantResetPage"]))
                {
                    return ConfigurationManager.AppSettings["MultiTenantResetPage"];
                }

                throw new Exception("MultiTenantResetPage not configured in the web config file");
            }
        }

        public static string MultiTenantSolrCoreUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantSolrCoreUrl"]))
                {
                    return ConfigurationManager.AppSettings["MultiTenantSolrCoreUrl"];
                }

                throw new Exception("MultiTenantSolrCoreUrl not configured in the web config file");
            }
        }

        public static string DateFormats
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemDateFormats"]))
                {
                    return ConfigurationManager.AppSettings["SystemDateFormats"];
                }

                throw new Exception("SystemDateFormats not configured in the web config file");
            }
        }

        public static string DateFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SystemDateFormat"]))
                {
                    return ConfigurationManager.AppSettings["SystemDateFormat"];
                }

                throw new Exception("SystemDateFormat not configured in the web config file");
            }
        }

        public static string SchemaName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SchemaName"]))
                {
                    return ConfigurationManager.AppSettings["SchemaName"];
                }

                throw new Exception("Schema Name not configured in the web config file");
            }
        }

        public static string OracleSchemaName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OracleSchemaName"]))
                {
                    return ConfigurationManager.AppSettings["OracleSchemaName"];
                }

                throw new Exception("Oracle Schema Name not configured in the web config file");
            }
        }

        public static string MultiTenantSchemaName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantSchemaName"]))
                {
                    return ConfigurationManager.AppSettings["MultiTenantSchemaName"];
                }

                throw new Exception("Multi Tenant Schema Name not configured in the web config file");
            }
        }

        public static bool IsOracleMigrationEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["IsOracleMigrationEnabled"].TryGetBoolValue();
            }
        }

        public static string SchemaNameDatabaseType
        {
            get
            {
                string schemaName = SchemaName;
                if (IsOracleMigrationEnabled)
                {
                    schemaName = OracleSchemaName;
                }
                return schemaName;
            }
        }

        public static string SolrServicePath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SolrServicePath"]))
                {
                    return ConfigurationManager.AppSettings["SolrServicePath"];
                }

                throw new Exception("Solr Service Path not configured in the web config file");
            }
        }

        public static string SolrServiceTenantsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SolrServiceTenantsPath"]))
                {
                    return ConfigurationManager.AppSettings["SolrServiceTenantsPath"];
                }

                throw new Exception("Solr Service Tenants Path not configured in the web config file");
            }
        }

        public static string DoconutIframeUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DoconutIframeUrl"]))
                {
                    return ConfigurationManager.AppSettings["DoconutIframeUrl"];
                }
                throw new Exception("DoconutIframeUrl  Path not configured in the web config file");
            }
        }

        public static bool MultiTenantCreateDatabaseEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantCreateDatabaseEnabled"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["MultiTenantCreateDatabaseEnabled"]);
                }

                throw new Exception("MultiTenantCreateDatabaseEnabled not configured in the web config file");
            }
        }
        public static bool IsWCF
        {
            get
            {
                return ConfigurationManager.AppSettings["IsWCF"].TryGetBoolValue();
            }
        }
        public static int RetrialsInterval
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["RetrialsInterval"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RetrialsInterval"]);
                }

                throw new Exception("RetrialsInterval is not configured in the web config file");
            }
        }

        public static int NumberOfRetrials
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NumberOfRetrials"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["NumberOfRetrials"]);
                }

                throw new Exception("NumberOfRetrials is not configured in the web config file");
            }
        }

        public static string ThumbPrint
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ThumbPrint"]))
                {
                    return ConfigurationManager.AppSettings["ThumbPrint"].ToString();
                }

                throw new Exception("ThumbPrint is not configured in the web config file");
            }
        }

        public static string HubDateFormat
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HubDateFormat"]))
                {
                    return ConfigurationManager.AppSettings["HubDateFormat"].ToString();
                }

                throw new Exception("HubDateFormat is not configured in the web config file");
            }
        }

        public static string HubSourceAgency
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HubSourceAgency"]))
                {
                    return ConfigurationManager.AppSettings["HubSourceAgency"].ToString();
                }

                throw new Exception("HubSourceAgency is not configured in the web config file");
            }
        }

        public static bool ShouldLockout
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ShouldLockout"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["ShouldLockout"].ToLower());
                }

                throw new Exception("ShouldLockout not configured in the web config file");
            }
        }

        public static bool EnableSSL
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableSSL"]))
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToLower());
                }
                throw new Exception("EnableSSL not configured in the web config file");
            }
        }

        public static string ReservationInboundDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReservationInboundDefault"]))
                {
                    return ConfigurationManager.AppSettings["ReservationInboundDefault"].ToString();
                }

                throw new Exception("ReservationInboundDefault not configured in the web config file");
            }
        }

        public static string ReservationOutboundDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReservationOutboundDefault"]))
                {
                    return ConfigurationManager.AppSettings["ReservationOutboundDefault"].ToString();
                }

                throw new Exception("ReservationOutboundDefault not configured in the web config file");
            }
        }

        public static string SupportEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SupportEmail"]))
                {
                    return ConfigurationManager.AppSettings["SupportEmail"].ToString();
                }

                throw new Exception("SupportEmail not configured in the web config file");
            }
        }

        public static int FaliureSendSupportAttemptsLimts
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FaliureSendSupportAttemptsLimts"]))
                {
                    return int.Parse(ConfigurationManager.AppSettings["FaliureSendSupportAttemptsLimts"].ToString());
                }

                throw new Exception("SupportEmail not configured in the web config file");
            }
        }

        public static int TaskProcessingPeriod
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TaskProcessingPeriod"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["TaskProcessingPeriod"]);
                }

                throw new Exception("TaskProcessingPeriod is not configured in the web config file");
            }
        }
        public static int TaskReminderCount
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TaskReminderCount"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["TaskReminderCount"]);
                }

                throw new Exception("TaskReminderCount is not configured in the web config file");
            }
        }
        public static int MaxAllowedReservation
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MaxAllowedReservation"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MaxAllowedReservation"]);
                }

                throw new Exception("MaxAllowedReservation is not configured in the web config file");
            }
        }

        public static bool IsNotificationEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["IsNotificationEnabled"].TryGetBoolValue();
            }
        }

        public static int ExternalPartyAttachmentTypeId
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExternalPartyAttachmentTypeId"]))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ExternalPartyAttachmentTypeId"]);
                }

                throw new Exception("ExternalPartyAttachmentTypeId is not configured in the web config file");
            }
        }

        public static int NotificationDateRange
        {
            get
            {
                return ConfigurationManager.AppSettings["NotificationDateRange"].TryGetIntValue(1);
            }
        }

        public static string HubSourceID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HubSourceID"]))
                {
                    return ConfigurationManager.AppSettings["HubSourceID"].ToString();
                }

                throw new Exception("HubSourceID is not configured in the web config file");
            }
        }

        public static string HubServiceID
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HubServiceID"]))
                {
                    return ConfigurationManager.AppSettings["HubServiceID"].ToString();
                }

                throw new Exception("HubServiceID is not configured in the web config file");
            }
        }

        public static string HubSourceName
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HubSourceName"]))
                {
                    return ConfigurationManager.AppSettings["HubSourceName"].ToString();
                }

                throw new Exception("HubSourceName is not configured in the web config file");
            }
        }

        public static int OTPExpirationMinutes
        {
            get
            {
                return ConfigurationManager.AppSettings["OTPExpirationMinutes"].TryGetIntValue(1);
            }
        }

        public static string EncryptionCertificateThumbprint
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["EncryptionCertificateThumbprint"]))
                {
                    return ConfigurationManager.AppSettings["EncryptionCertificateThumbprint"].ToString();
                }

                throw new Exception("EncryptionCertificateThumbprint is not configured in the web config file");
            }
        }

        public static string VisitTicketFooter
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["VisitTicketFooter"]))
                {
                    return ConfigurationManager.AppSettings["VisitTicketFooter"].ToString();
                }

                throw new Exception("VisitTicketFooter not configured in the web config file");
            }
        }

        public static string ERPConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPConnectionString"]))
                {
                    return ConfigurationManager.AppSettings["ERPConnectionString"];
                }

                throw new Exception("ERPConnectionString not configured in the web config file");
            }
        }

        public static string ERPPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPPassword"]))
                {
                    return ConfigurationManager.AppSettings["ERPPassword"];
                }

                throw new Exception("ERPPassword not configured in the web config file");
            }
        }

        public static string ERPProcessingPeriod
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPProcessingPeriod"]))
                {
                    return ConfigurationManager.AppSettings["ERPProcessingPeriod"];
                }

                throw new Exception("ERPProcessingPeriod not configured in the web config file");
            }
        }

        public static string ERPTitleId
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPTitleId"]))
                {
                    return ConfigurationManager.AppSettings["ERPTitleId"];
                }

                throw new Exception("ERPTitleId not configured in the web config file");
            }
        }

        public static string ERPCategoryId
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPCategoryId"]))
                {
                    return ConfigurationManager.AppSettings["ERPCategoryId"];
                }

                throw new Exception("ERPCategoryId not configured in the web config file");
            }
        }

        public static string ERPAdminGroupId
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPAdminGroupId"]))
                {
                    return ConfigurationManager.AppSettings["ERPAdminGroupId"];
                }

                throw new Exception("ERPAdminGroupId not configured in the web config file");
            }
        }

        public static string ERPEditorGroupId
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPEditorGroupId"]))
                {
                    return ConfigurationManager.AppSettings["ERPEditorGroupId"];
                }

                throw new Exception("ERPEditorGroupId not configured in the web config file");
            }
        }

        public static bool ERPIntegrationEnabled
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ERPIntegrationEnabled"]))
                {
                    return ConfigurationManager.AppSettings["ERPIntegrationEnabled"].TryGetBoolValue();
                }

                throw new Exception("ERPIntegrationEnabled not configured in the web config file");
            }
        }

        public static int ComplaintUnitId
        {
            get
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ComplaintUnitId"].ToString()))
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["ComplaintUnitId"].ToString());
                }

                throw new Exception("ComplaintUnitId not configured in the web config file");
            }
        }
        public static string WordAddInStoragePath
        {
            get
            {
                return ConfigurationManager.AppSettings["WordAddInStoragePath"].ToString();
            }
        }
        public static string VipAssignmentPaperActions
        {
            get
            {
                return ConfigurationManager.AppSettings["VipAssignmentPaperActions"].ToString();
            }
        }
        public static string AssignmentPaperActionsIds
        {
            get
            {
                return ConfigurationManager.AppSettings["AssignmentPaperActionsIds"].ToString();
            }
        }

        public static string WebBaseUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebHostName"].ToString()))
                {
                    return ConfigurationManager.AppSettings["WebHostName"].ToString();
                }
                return null;
            }
        }


        public static string SapUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SapUrl"].ToString()))
                {
                    return ConfigurationManager.AppSettings["SapUrl"].ToString();
                }
                return null;
            }
        }

        public static string SapToken
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["SapToken"].ToString()))
                {
                    return ConfigurationManager.AppSettings["SapToken"].ToString();
                }
                return null;
            }
        }
        public static string DefaultConnectionString
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultConnectionString"].ToString()))
                {
                    return ConfigurationManager.AppSettings["DefaultConnectionString"].ToString();
                }
                return null;
            }
        }
        
        public static string CodeExpirationDuration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ExpirationCodeInSec"].ToString()))
                {
                    return ConfigurationManager.AppSettings["ExpirationCodeInSec"].ToString();
                }
                return null;
            }
        }

        public static string OfficeUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["OfficeUrl"].ToString()))
                {
                    return ConfigurationManager.AppSettings["OfficeUrl"].ToString();
                }
                return null;
            }
        }
    }
}
