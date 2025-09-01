namespace MCS.Common
{
    public class UserClaims
    {
        public static class Admin
        {
            private const string Prefix = "SystemAdminstration.";
            public const string Administrator = Prefix + "Admin";
            public const string SuperAdministrator = Prefix + "Super.Admin";
            public const string LookupsUnLock = Prefix + "Lookups.UnLock";
            public const string AdminLookups = Prefix + "Lookups";
            public const string AdminOrgUnitStructure = Prefix + "OrgUnitStructure";
            public const string AdminGeneralSettings = Prefix + "GeneralSettings";
            public const string AdminExternalParties = Prefix + "ExternalParties";
            public const string AdminReleaseNotes = Prefix + "ReleaseNotes";
            public const string ManageUsers = Prefix + "ManageUsers";
            public const string MoveManagement = Prefix + "MoveManagement";
        }

        //public static class Editor
        //{
        //public const string Prefix = "Editor";
        //public const string ViewEditor = "Editor.Editor";
        //public const string Inbound = "Editor.Inbound";
        //public const string Draft = "Editor.Draft";
        //public const string Explanations = "Editor.Explanations";
        //public const string ViewTransactions = "Editor.ViewTransactions";
        //public const string Link = "Editor.Link";
        //public const string Assignments = "Editor.Assignments";
        //public const string AssignmentPaper = "Editor.AssignmentPaper";
        //}

        public static class Expalanations
        {
            public const string Prefix = "Confidentiality.Explanations";
            public const string General = "Confidentiality.Explanations.General";
            public const string Secret = "Confidentiality.Explanations.Secret";
            public const string ExtremlyConfidential = "Confidentiality.Explanations.ExtremlyConfidential";
        }

        public static class Tasks
        {
            public const string Prefix = "Tasks";
            public const string Add = "Tasks.Add";
            public const string Delete = "Tasks.Delete";
            //public const string Sequence = "Tasks.Sequencing";
            //public const string Exclusive = "Tasks.Exclusive";
            //public const string Tray = "Tasks.Tray";
            public const string Reminder = "Tasks.Reminder";
            //public const string EditDate = "Tasks.EditDate";
            //public const string Details = "Tasks.Details";
            //public const string Sub = "Tasks.SubTasking";
            public const string Edit = "Tasks.Edit";
            public const string Replay = "Tasks.Reply";
            public const string Resend = "Tasks.Resend";
            public const string DisplayLink = "Tasks.Tasks";

        }

        public static class Links
        {
            public const string Prefix = "Link";

            public const string DisplayLink = "Link.Link";
            public const string Add = "Link.AddLink";
            public const string Delete = "Link.DeleteLink";
            public const string Edit = "Link.EditLinks";
            public const string GeneralLink = "Link.GeneralLink";
        }

        public static class Files
        {
            public const string Prefix = "File";
            public const string File = "File.File";
            public const string SaveRetrieve = "File.SaveRetrieve";
            public const string MyTransactions = "File.MyTransactions";
            public const string SentTransactions = "File.SentTransactions";
            public const string DraftOutbound = "File.DraftOutbound";
            public const string Saved = "File.Saved";
            public const string OrgUnit = "File.OrgUnite";
            public const string Manager = "File.Manager";
            public const string Copies = "File.Copies";
            public const string YESSER = "File.YESSER";
            public const string Tasks = "File.Tasks";
            public const string FollowUp = "File.FollowUp";
            public const string Reservation = "File.Reservation";
            public const string CopiesOutbound = "File.CopiesOutbound";
            public const string OutboundExternal = "File.OutboundExternal";
            public const string ElcOutbound = "File.OutboundExternal";
            public const string ReservedExternalOutbound = "File.ReservedExternalOutbound";
            public const string Withdrawal = "File.Withdrawal";
            public const string Archives = "File.Archives";

        }



        public static class Inbound
        {
            public const string Prefix = "Inbound";
            public const string DisplayInbound = "Inbound.Inbound";
            public const string CreateInbound = "Inbound.CreateInbound";
            public const string EditInbound = "Inbound.EditInbound";
            //public const string AddAttachments = "Inbound.AddAttachments";
            //public const string DeleteAttachments = "Inbound.DeleteAttachments";
            //public const string AddNames = "Inbound.AddNames";
            //public const string DeleteNames = "Inbound.DeleteNames";
            public const string PreviousData = "Inbound.PreviousData";
            public const string Editor = "Inbound.Editor";

        }

        public static class UserCategory
        {
            public const string Prefix = "UserCategory";
            public const string Management = "UserCategory.Management";
            public const string CommunicationsUser = "UserCategory.CommunicationsUser";
            public const string Editor = "UserCategory.Editor";
            public const string HeadOfDepartment = "UserCategory.HeadOfDepartment";
            public const string DecisionMaker = "UserCategory.DecisionMaker";
        }

        public static class Outbound
        {
            public const string Prefix = "Outbound";
            public const string DisplayOutbound = "Outbound.Outbound";
            public const string CreateExternalOutbound = "Outbound.CreateExternalOutbound";
            public const string EditOutbound = "Outbound.EditOutbound";
            //public const string AddCopies = "Outbound.AddCopies";
            //public const string DeleteCopies = "Outbound.DeleteCopies";
            //public const string EditCopies = "Outbound.EditCopies";
            //public const string AddAttachments = "Outbound.AddAttachments";
            //public const string DeleteAttachments = "Outbound.DeleteAttachments";
            //public const string AddNames = "Outbound.AddNames";
            //public const string DeleteNames = "Outbound.DeleteNames";
            //public const string InternalOutbound = "Outbound.InternalOutbound";
            //public const string DistributionLists = "Outbound.DistributionLists";
            public const string CreateOutboundDraft = "Outbound.CreateOutboundDraft";
            public const string CreateOutboundDraftPresentation = "Outbound.CreateOutboundDraftPresentation";
            public const string EditDraft = "Outbound.EditOutboundDraft";
            public const string CreateDecisionDraft = "Outbound.CreateDecisionDraft";
            public const string CreateMultiOutbound = "Outbound.CreateMultiOutbound";
            public const string PrintTransaction = "Outbound.PrintTransaction";
            public const string DownloadTransaction = "Outbound.Download";



            #region OutboundInternal
            public const string CreateInternalOutbound = "Outbound.CreateInternalOutbound";
            public const string CreateMultiInternalOutbound = "Outbound.CreateMultiInternalOutbound";
            public const string CreateInternalOutboundFromCopy = "Outbound.CreateInternalOutboundFromCopy";
            //public const string PreviousData = "Outbound.PreviousData";

            public const string EditInternalOutbound = "Outbound.EditInternalOutbound";
            public const string EditorInternalOutbound = "Outbound.InternalOutboundEditor";
            #endregion
        }

        public static class Transactiontypes
        {
            public const string Prefix = "Transactiontypes";
            public const string General = "Transactiontypes.General";
            public const string Secret = "Transactiontypes.Secret";
            public const string Note = "Transactiontypes.Note";
            public const string Decision = "Transactiontypes.Decision";
        }

        public static class Search
        {
            public const string Prefix = "Search";
            public const string DisplaySearch = "Search.Search";
            public const string SearchbyTransactionNumberInboundOutbound = "Search.SearchbyTransactionNumberInboundOutbound";
            public const string SearchbyEncryptionCode = "Search.SearchbyEncryptionCode";
            public const string SearchbySubject = "Search.SearchbySubject";
            public const string SearchByAssignTransaction = "Search.SearchByAssignTransaction";
            public const string SearchByInboundNumber = "Search.SearchByInboundNumber";
            public const string SearchByOutboundNumber = "Search.SearchByOutboundNumber";
            public const string SearchByOutboundInternalNumber = "Search.SearchByOutboundInternalNumber";
            public const string SearchByOutboundDraftNumber = "Search.SearchByOutboundDraftNumber";
            public const string SearchByEntity = "Search.SearchByEntity";
            public const string SearchByCreator = "Search.SearchByCreator";
            public const string SearchByDocumentNumber = "Search.SearchByDocumentNumber";
            public const string SearchByRecordNumber = "Search.SearchByRecordNumber";
            public const string SearchAllTransactions = "Search.ShowAllTransactions";
            public const string InquirybyTransactionNumber = "Search.InquirybyTransactionNumber";
            public const string GlobalSearch = "Search.GlobalSearch";
            public const string GeneralInquiry = "Search.GeneralInquiry";
            public const string SearchAll = "Search.SearchByALL";
            public const string SearchByNames = "Search.Names";
            public const string SearchDaily = "Search.Daily";
            public const string SearchByAssignmentNote = "Search.AssignmentNote";
            public const string SearchByManifestNumber = "Search.ManifestNumber";
            public const string SearchByMilitaryNumberOrIdentity = "Search.MilitaryNumberOrIdentity";
            public const string SearchByTransactionNots = "Search.TransactionNots";
            public const string SearchByELcEmployee = "Search.ELcEmployee";
            public const string SearchByExternalOutBoundOrManifestNumber = "Search.ExternalOutBoundOrManifestNumber";
            public const string SearchByCopyAssignemnt = "Search.CopyAssignemnt";
            public const string SearchBySubjectLetter = "Search.SubjectLetter";
            public const string SearchByTransactionNumber = "Search.TransactionNumber";
            public const string SearchAllModules = "Search.SearchAllModules";
            public const string SearchAllChildsModules = "Search.SearchAllChildsModules";
            public const string SearchParentDepartment = "Search.SearchParentDepartment";
            public const string ChangeSubject = "Search.ChangeSubject";
            public const string SearchByExternalPartyCopies = "Search.ExternalPartyCopies";

        }

        public static class Hub
        {
            public const string Prefix = "Hub";
            public const string GetHubTransactions = "Hub.GetHubTransactions";
            public const string ContactHub = "Hub.ContactHub";
        }

        //public static class FollowUpDepartments
        //{
        //    public const string Prefix = "Departments";
        //    public const string AllDepartments = "Departments.AllDepartments";
        //    public const string OnlyUserDepartment = "Departments.OnlyUserDepartment";
        //    public const string ConfidentialityDepartments = "Departments.ConfidentialityDepartments";
        //}

        public static class Reports
        {
            public const string Prefix = "Reports";
            public const string DisplayReports = "Reports.Reports";
            public const string InboundTransactionsReports = "Reports.InboundTransactionsReports";
            public const string OutboundTransactionsReports = "Reports.OutboundTransactionsReports";
            public const string StatisticalReportsOfTransactions = "Reports.StatisticalReportsOfTransactions";
            public const string UserPerformanceReports = "Reports.UserPerformanceReports";
            public const string SaveReports = "Reports.SaveReports";
            public const string PrintReports = "Reports.PrintReports";
            public const string TransactionReports = "Reports.TransactionReports";
            public const string PerformanceMeasurementReports = "Reports.PerformanceMeasurementReports";
            public const string userTransactionsReports = "Reports.UserTransactionsReports";
            public const string Bussinesintelligence = "Reports.Bi";
            public const string ViewOrganizationUnit = "Reports.ViewOrganizationUnit";
            public const string SecretaryTransactionReport = "Reports.SecretaryTransactionReports";
            public const string ReportsAllModules = "Reports.ReportAllModules";
            public const string ReportsAllChildsModules = "Reports.ReportAllChildsModules";
            public const string ReportsParentDepartment = "Reports.ReportParentDepartment";
            public const string ReportsParentWithChildDepartment = "Reports.ReportParentWithChildDepartment";
            public const string DashboardAllModules = "Reports.DashboardAllModules";
            public const string DashboardAllChildsModules = "Reports.DashboardAllChildsModules";
            public const string DashboardParentDepartment = "Reports.DashboardParentDepartment";
            public const string DashboardParentWithChildDepartment = "Reports.DashboardParentWithChildDepartment";
        }

        public static class AddExternalParty
        {
            public const string Prefix = "AddExternalParty";
            public const string AddExternalPartyForOutbound = "AddExternalParty.ForOutbound";
            public const string AddExternalPartyForInbound = "AddExternalParty.ForInbound";
        }

        public static class ConfidentialityOfTransactions
        {
            public const string Prefix = "Confidentiality.Transactions";
            public const string General = "Confidentiality.Transactions.General";
            public const string Secret = "Confidentiality.Transactions.Secret";
            public const string ExtremlyConfidential = "Confidentiality.Transactions.ExtremlyConfidential";
            public const string SecretAndLimitedTrading = "Confidentiality.Transactions.SecretAndLimitedTrading";
            public const string HandDelivered = "Confidentiality.Transactions.HandDelivered";
        }

        public static class PrivacyOfTransactions
        {
            public const string Prefix = "Privacy.Transactions";
            public const string Private = "Privacy.Transactions.Private";
            public const string Limited = "Privacy.Transactions.Limited";
            public const string OpenByHand = "Privacy.Transactions.OpenByHand";
        }

        public static class ModulesLevel
        {
            public const string Prefix = "ModulesLevel";
            public const string AllModules = "ModulesLevel.AllModules";
            public const string AllChildsModules = "ModulesLevel.AllChildsModules";
            public const string ParentDepartment = "ModulesLevel.ParentDepartment";
            public const string ParentWithChildDepartment = "ModulesLevel.ParentWithChildDepartment";
        }

        public static class Assignments
        {
            public const string Prefix = "Assignments";
            public const string Assign = "Assignments.Assign";
            public const string AssignTo = "Assignments.AssignTo";
            public const string AssignToOtherDepartment = "Assignments.AssignToOtherDepartment";
            public const string AssignToEmployeeInOtherDepartment = "Assignments.AssignToEmployeeInOtherDepartment";
            public const string WithdrawTransaction = "Assignments.WithdrawTransaction";
            public const string WithdrawTransactionFromTidyCabins = "Assignments.WithdrawTransactionFromTidyCabins";
            public const string WithdrawTransactionFromAllCabins = "Assignments.WithdrawTransactionFromAllCabins";
            public const string WhiteList = "Assignments.WhiteList";
            public const string Save = "Assignments.Save";
            public const string Approve = "Assignments.Approve";
        }

        public static class Archiving
        {
            public const string Prefix = "Archiving";
            //public const string DisplayArchiving = "Archiving.Archiving";
            //public const string AddArchiving = "Archiving.AddArchiving";
            public const string DeleteArchiving = "Archiving.DeleteArchiving";
            public const string PrintArchiving = "Archiving.PrintArchiving";
            public const string DownLoadArchiving = "Archiving.DownLoadArchiving";
            public const string EditArchiving = "Archiving.EditArchiving";
            //public const string PreviewArchiving = "Archiving.PreviewArchiving";
            //public const string General = "Archiving.General";
            //public const string Secret = "Archiving.Secret";
            //public const string ExtremlyConfidential = "Archiving.ExtremlyConfidential";
            public const string ColorScanning = "Archiving.ColorScanning";
            public const string Annotations = "Archiving.Annotations";
        }

        public static class DashboardsChart
        {
            public const string Prefix = "DashboardsChart";
            public const string TransactionsCountInSystem = "DashboardsChart.TransactionsCountInSystem";
            public const string TransactionsCountByUser = "DashboardsChart.TransactionsCountByUser";
            public const string TransactionsCountByDay = "DashboardsChart.TransactionsCountByDay";
        }


        public static class GeneralPermissions
        {
            public const string Prefix = "GeneralPermissions";
            //public const string TransactionMovment = "GeneralPermissions.TransactionMovment";
            public const string PrintEncryptionCode = "GeneralPermissions.PrintEncryptionCode";
            //public const string EditNames = "GeneralPermissions.EditNames";
            public const string PrintReviewTicket = "GeneralPermissions.PrintReviewTicket";
            public const string PrintArchiveTransaction = "GeneralPermissions.PrintArchiveTransaction";
            public const string PrintArchiveTransactionLink = "GeneralPermissions.PrintArchiveTransactionLink";
            //public const string FollowUpTransactionDate = "GeneralPermissions.FollowUpTransactionDate";
            public const string PrintDeliveryData = "GeneralPermissions.PrintDeliveryData";
            //public const string PrintTransactionGeneralInfo = "GeneralPermissions.PrintTransactionGeneralInfo";
            public const string ReturnTransaction = "GeneralPermissions.ReturnTransaction";
            public const string WaterMark = "GeneralPermissions.WaterMark";
            public const string AdministrationCreated = "ShouldBeDeletedYousef";
            public const string PreviewArchiving = "GeneralPermissions.PreviewArchiving";
            public const string DeletedDraftOutbound = "GeneralPermissions.DeletedDraftOutbound";
            public const string UndoDeletedDraftOutbound = "GeneralPermissions.UndoDeletedDraftOutbound";
            public const string CancelElcOutbound = "GeneralPermissions.CancelElcOutbound";
            public const string ManageTemplate = "GeneralPermissions.ManageTemplate";
            public const string PrintEncryptionCodeCreation = "GeneralPermissions.PrintEncryptionCodeCreation";
            public const string ApprovalTransaction = "GeneralPermissions.ApprovalTransaction";
            public const string AssignTransactionToUnauthorize = "GeneralPermissions.AssignTransactionToUnauthorize";
            public const string GenerateKeyAPI = "GeneralPermissions.GenerateKeyAPI";
            public const string OnlineUser = "GeneralPermissions.OnlineUser";
            public const string EncryptTransaction = "GeneralPermissions.EncryptTransaction";
            public const string OpenEncryptTransaction = "GeneralPermissions.OpenEncryptTransaction";
            public const string RequestRole = "GeneralPermissions.RequestRole";
            public const string Survey = "GeneralPermissions.Survey";

        }


        public static class SystemAdministration
        {
            public const string Prefix = "SystemAdministration";
            public const string SystemAdmin = "SystemAdminstration.SystemAdmin";
        }

        //public static class OutboundTransactionsTypes
        //{
        //    public const string Prefix = "OutboundTransactionsTypes";
        //    public const string General = "OutboundTransactionsTypes.General";
        //    public const string Secret = "OutboundTransactionsTypes.Secret";
        //}

        //public static class InboundTransactionsTypes
        //{
        //    public const string Prefix = "InboundTransactionsTypes";
        //    public const string General = "InboundTransactionsTypes.General";
        //    public const string Secret = "InboundTransactionsTypes.Secret";
        //}

        //public static class InternalOutboundTransactionsTypes
        //{
        //    public const string Prefix = "InternalOutboundTransactionsTypes";
        //    public const string General = "InternalOutboundTransactionsTypes.General";
        //    public const string Secret = "InternalOutboundTransactionsTypes.Secret";
        //}

        //public static class AssignmentPaper
        //{
        //    public const string Prefix = "AssignmentPaper";
        //    public const string EditAssignmentPaper = "AssignmentPaper.EditAssignmentPaper";
        //    public const string AddTask = "AssignmentPaper.AddTask";
        //    public const string CreateGroup = "AssignmentPaper.CreateGroup";
        //    public const string ViewAssignmentPaper = "AssignmentPaper.ViewAssignmentPaper";
        //}
        public static class Reporter
        {
            public const string Prefix = "Transaction";
            public const string AddReporter = "Transaction.AddReporter";
        }
        public static string GetClaimPrefix(string claimCode)
        {
            int separatorIndex = claimCode.IndexOf(".");

            return claimCode.Substring(0, separatorIndex);
        }

        public static string GenerateClaimCode(string prefixClaim, string claimName)
        {
            return string.Format(prefixClaim + "{0}" + claimName, ".");
        }

        public static class Dashboards
        {
            public const string Prefix = "Dashboard";
            public const string Dashboard = "Dashboard.Dashboard";
            //public const string Entity = "Dashboard.Entity";
            //public const string ChiledEntities = "Dashboard.ChiledEntities";
            //public const string AllEntities = "Dashboard.AllEntities";
        }
        public static class FollowUps
        {
            public const string Prefix = "Followup";
            public const string DisplayLink = "Followup.Followup";
            public const string AddPublicFollowUp = "Followup.AddPublic";
            public const string AddPrivetFollowUp = "Followup.AddPrivet";
            public const string AddFollowUp = "Followup.Add";
            public const string SaveFollowUp = "Followup.Save";
            public const string DeleteFollowUp = "Followup.Delete";
            public const string SearchAddEdit = "Followup_AddDelete_FromSearch";
            public const string FollowUpEscalation = "Followup.File.Ecalation";
            public const string FollowUpNew = "Followup.File.New";
            public const string FollowUpUnderProccess = "Followup.File.UnderProccess";
            public const string FollowUpComplete = "Followup.File.Complete";
            public const string FollowUpLate = "Followup.File.Late";
            public const string FollowUpReminder = "Followup.File.Reminder";
            public const string FollowUpCanceld = "Followup.File.Canceld";

        }


        public static class IC
        {
            public const string ICMain = "IC";
            public const string ICAdd = "IC.Add";
            public const string ICUpdate = "IC.Update";
            public const string ICDelete = "IC.Delete";
            public const string ICClassification = "IC.Classification";
        }

        public static class Names
        {
            public const string Prefix = "Names";

            public const string DisplayLink = "Name.Name";
            public const string Add = "Name.AddName";
            public const string Delete = "Name.DeleteName";
            public const string Edit = "Name.EditName";
        }

        public static class CopiesInternal
        {
            public const string Prefix = "CopiesInternal";

            public const string DisplayLink = "CopiesInternal.CopiesInternal";
            public const string Add = "CopiesInternal.Add";
            public const string Delete = "CopiesInternal.Delete";
            public const string Edit = "CopiesInternal.Edit";
            public const string BCC = "Copies.BCC";
        }

        public static class CopiesExternal
        {
            public const string Prefix = "CopiesExternal";

            public const string DisplayLink = "CopiesExternal.CopiesExternal";
            public const string Add = "CopiesExternal.Add";
            public const string Delete = "CopiesExternal.Delete";
            public const string Edit = "CopiesExternal.Edit";
            public const string Attachment = "CopiesExternal.Attachment";
        }

        public static class ExpalanationsEditor
        {
            public const string Prefix = "Explainations";

            public const string DisplayLink = "Explainations.Expalanations";
            public const string Add = "Explainations.Add";
            public const string Delete = "Explainations.Delete";
            public const string Edit = "Explainations.Edit";
        }

        public static class UserPreferences
        {
            public const string Prefix = "UserPreferences";

            public const string Language = "UserPreferences.Language";
            public const string Signature = "UserPreferences.Signature";
            public const string SignatureCommand = "UserPreferences.SignatureCommand";
            public const string SignatureBehalf = "UserPreferences.SignatureBehalf";
            public const string Mark = "UserPreferences.Mark";
            public const string MessageSignature = "UserPreferences.MessageSignature";
            public const string SealSignature = "UserPreferences.SealSignature";
            public const string AssignmentPaper = "UserPreferences.AssignmentPaper";
            public const string DistributionList = "UserPreferences.DistributionList";
            public const string Theme = "Themes.Themes";
        }

        public static class TransactionCertificate
        {
            public const string Prefix = "TransactionCertificate";

            public const string Names = "TransactionCertificate.Names";
            public const string Assignments = "TransactionCertificate.Assignments";
            public const string Attachments = "TransactionCertificate.Attachments";
            public const string CopiesInternal = "TransactionCertificate.CopiesInternal";
            public const string CopiesExternal = "TransactionCertificate.CopiesExternal";
            public const string Links = "TransactionCertificate.Links";
            public const string Explanations = "TransactionCertificate.Explanations";
            public const string Followup = "TransactionCertificate.Followup";
            public const string Tasks = "TransactionCertificate.Tasks";
            public const string Modifications = "TransactionCertificate.Modifications";
            public const string Tracking = "TransactionCertificate.Tracking";

        }
        public static class PopUpWindowData
        {
            public const string Prefix = "PopUpWindow";
            public const string Inbound_PrintTicket = "PopUpWindow.Inbound.PrintTicket";
            public const string Inbound_PrintBarCode = "PopUpWindow.Inbound.PrintBarCode";
            public const string Inbound_AddArchive = "PopUpWindow.Inbound.AddArchive";
            public const string Inbound_CopyData = "PopUpWindow.Inbound.CopyData";

            public const string Outbound_PrintBarCode = "PopUpWindow.Outbound.PrintBarCode";
            public const string Outbound_PrintTitle = "PopUpWindow.Outbound.PrintTitle";
            public const string Outbound_AddArchive = "PopUpWindow.Outbound.AddArchive";
            public const string Outbound_CopyData = "PopUpWindow.Outbound.CopyData";
            public const string Outbound_SendOutbound = "PopUpWindow.Outbound.SendOutbound";

            public const string Internal_PrintBarCode = "PopUpWindow.Internal.PrintBarCode";
            public const string Internal_AddArchive = "PopUpWindow.Internal.AddArchive";
            public const string Internal_CopyData = "PopUpWindow.Internal.CopyData";

            public const string Draft_PrintBarCode = "PopUpWindow.Draft.PrintBarCode";
            public const string Draft_AddArchive = "PopUpWindow.Draft.AddArchive";
            public const string Draft_CopyData = "PopUpWindow.Draft.CopyData";

        }
        public static class SurveyReport
        {
            public const string UserSuggestionsReport = "Survey.UserSuggestionsReport";
            public const string UserReport = "Survey.UserReport";
        }
    }
}
