using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class TransactionApiHelper
    {
        public ITransactionApi TransactionApiClient { get; }
        public TransactionApiHelper()
        {
            TransactionApiClient = ClientFactory.GetClient<ITransactionApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<GetResult<List<NotificationDTO>>> GetNotifications(SearchCriteria searchCriteria, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetNotifications(searchCriteria, cultureName);
            return result;
        }

        public static async Task<DeleteResult> DeleteNotifications(string ids)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.DeleteNotifications(ids);
            return result;
        }

        public static async Task<PostObjectResult<TransactionDetailsDTO>> PostTransaction(TransactionDTO transactionDTO, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostTransaction(transactionDTO, cultureName);
            return result;
        }


        public static async Task<PutResult> PutTransaction(string cultureName, TransactionDTO transactionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PutTransaction(cultureName, transactionDTO);
            return result;
        }

        public static async Task<GetResult<TransactionTicketDTO>> PrintTransactionTicket(TransactionTicketInfoDTO transactionTicketInfoDTO, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PrintTransactionTicket(transactionTicketInfoDTO, cultureName);
            return result;
        }


        public static async Task<GetResult<TransactionBarcodesDTO>> GetTransactionBarcodes(int transactionId, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionBarcodes(transactionId, orgUnitId, cultureName);
            return result;
        }


        public static async Task<GetResult<List<DeliveryReportDTO>>> PrintDeliveryReportById(string strTransactionReportInfos, string cultureName, bool perTransaction = true)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PrintDeliveryReportById(strTransactionReportInfos, cultureName, perTransaction);
            return result;
        }


        public static async Task<GetResult<List<DeliveryReportDTO>>> PrintDeliveryReport(int transactionId, string cultureName, bool perTransaction = true)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PrintDeliveryReport(transactionId, cultureName, perTransaction);
            return result;
        }


        public static async Task<GetResultExtraData<object>> GetTransactionCertificateById(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionCertificateById(transactionId, cultureName);
            return result;
        }


        public static async Task<GetResultExtraData<object>> GetTransactionCertificateByReference(string referenceCode, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionCertificateByReference(referenceCode, orgUnitId, cultureName);
            return result;
        }


        public static async Task<GetResult<InboundCertificateDTO>> GetInboundCertificate(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetInboundCertificate(transactionId, cultureName);
            return result;
        }


        public static async Task<GetResult<OutboundCertificateDTO>> GetOutboundCertificate(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetOutboundCertificate(transactionId, cultureName);
            return result;
        }


        public static async Task<GetResult<List<TransactionDeliveryReportDTO>>> SearchDeliveryReport(string strSearchCriteria)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.SearchDeliveryReport(strSearchCriteria);
            return result;
        }


        public static async Task<GetResult<TransactionVisitTicketDTO>> GetTransactionVisitTicket(int transactionId, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionVisitTicket(transactionId, orgUnitId, cultureName);
            return result;
        }

        public static async Task<GetResult<TransactionNameDTO>> GetTransactionName(string civilID, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionName(civilID, cultureName);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> GetTransaction(int userId, int transactionNumber, TransactionCategory transactionType, int year, int sourceId, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransaction(userId, transactionNumber, transactionType, year, sourceId, orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<int>> GetTransactionIdByLinkType(int linkTypeId, string sourceNumber, int orgUnitId, int yearId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionIdByLinkType(linkTypeId, sourceNumber, orgUnitId, yearId, cultureName);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> GetPreviousTransaction(TransactionCategory transactionType, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetPreviousTransaction(transactionType, orgUnitId, cultureName);
            return result;
        }
        public static async Task<PostResult> PostTransactionAssignment(List<int> transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostTransactionAssignment(transactionId, transactionAssignmentDTOs, cultureName);
            return result;
        }
        public static async Task<PostResult> PostTransactionTasks(TransactionTaskDTO transactionTaskDTOs, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostTransactionTasks(transactionTaskDTOs, cultureName);
            return result;
        }
        public static async Task<PutResult> PutRejectTransactionTask(TaskActionDTO taskActionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PutRejectTransactionTask(taskActionDTO);
            return result;
        }
        public static async Task<PutResult> PutCompleteTransactionTask(TaskActionDTO taskActionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PutCompleteTransactionTask(taskActionDTO);
            return result;
        }
        public static async Task<PostResult> PostSubTransactionTask(TransactionSubTaskDTO transactionSubTaskDTO, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostSubTransactionTask(transactionSubTaskDTO, cultureName);
            return result;
        }

        public static async Task<GetResult<int?>> GetTasksCount(int assignmentId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTasksCount(assignmentId);
            return result;
        }

        public static async Task<GetResult<List<ReceivedTaskDTO>>> GetReceivedTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetReceivedTasks(pageIndex, pageSize, orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TaskStatusDTO>>> GetUserTasksStatus(int userId, int orgUnitId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetUserTasksStatus(userId, orgUnitId);
            return result;
        }
        public static async Task<GetResult<ReceivedTaskDTO>> GetReceivedTask(int taskId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetReceivedTask(taskId, cultureName);
            return result;
        }
        public static async Task<GetResult<SentTaskDTO>> GetSentTask(int taskId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetSentTask(taskId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<SentTaskDTO>>> GetSentTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetSentTasks(pageIndex, pageSize, orgUnitId, cultureName);
            return result;
        }
        public static async Task<PostResult> PostTaskReminder(int taskId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostTaskReminder(taskId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<OrgUnitDTO>>> GetTaskSequenceOrgUnits(int taskId, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTaskSequenceOrgUnits(taskId, orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetTaskSequenceUsers(int taskId, int fromOrgUnitId, int toOrgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTaskSequenceUsers(taskId, fromOrgUnitId, toOrgUnitId, cultureName);
            return result;
        }
        public static async Task<PostResult> ExtendTaskDate(int taskId, string dateTime)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.ExtendTaskDate(taskId, dateTime);
            return result;
        }
        public static async Task<GetResult<List<TaskAddDTO>>> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionTasks(transactionId, searchCriteria, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TrayDetailsDTO>>> GetUserTrays(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetUserTrays(orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<TrayDetailsDTO>> GetTrayDetailsInfo(TrayType trayType, int orgUnitId, SearchCriteria searchCriteria)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTrayDetailsInfo(trayType, orgUnitId, searchCriteria);
            return result;
        }
        public static async Task<GetResult<TrayDetailsDTO>> GetPopulariazations(int orgUnitId, SearchCriteria searchCriteria)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetPopulariazations(orgUnitId, searchCriteria);
            return result;
        }
        public static async Task<GetResult<List<TransactionTrayInfoDTO>>> GetUserTransactionsTray(TrayType trayType, int orgUnitId, TransactionDateType transactionDate, SearchCriteria searchCriteria)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetUserTransactionsTray(trayType, orgUnitId, transactionDate, searchCriteria);
            return result;
        }
        public static async Task<PutResult> MoveTransaction(int transactionId, int orgUnitId, int trayActionTypeId, int trayId, int? assigmentId, object extraParams)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.MoveTransaction(transactionId, orgUnitId, trayActionTypeId, trayId, assigmentId, extraParams);
            return result;
        }
        public static async Task<PostObjectResult<TransactionDetailsDTO>> CreateOutboundExternal(int transactionId, int trayId, TransactionDTO transactionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.CreateOutboundExternal(transactionId, trayId, transactionDTO);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> PrepareOutboundCreation(int transactionId, int orgUnitId, int trayId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PrepareOutboundCreation(transactionId, orgUnitId, trayId, cultureName);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> GetPriorities(TransactionCategory transactionType, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetPriorities(transactionType, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TransactionTypeDTO>>> GetTransactionTypes(TransactionCategory transactionType, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionTypes(transactionType, cultureName);
            return result;
        }
        public static async Task<GetResult<List<LinkDTO>>> GetLinkTypes(TransactionCategory transactionType, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetLinkTypes(transactionType, cultureName);
            return result;
        }
        public static async Task<GetResult<FormContentDTO>> GetContentByFormId(int formId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetContentByFormId(formId);
            return result;
        }
        public static async Task<GetResult<List<LetterTypeDTO>>> GetLetterTypes(TransactionCategory transactionType, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetLetterTypes(transactionType, cultureName);
            return result;
        }
        public static async Task<GetResult<List<AttachmentTypeDTO>>> GetAttachmentTypes(TransactionCategory transactionType, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetAttachmentTypes(transactionType, cultureName);
            return result;
        }
        public static async Task<GetResult<bool>> CheckOrgUnitHasAssignmentPaper(int orgUnitId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.CheckOrgUnitHasAssignmentPaper(orgUnitId);
            return result;
        }
        public static async Task<GetResult<bool>> CheckOrgUnitIsAllowedToCreateGroup(int orgUnitId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.CheckOrgUnitIsAllowedToCreateGroup(orgUnitId);
            return result;
        }
        public static async Task<GetResult<List<OrgUnitDTO>>> GetOrgUnitLinks(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetOrgUnitLinks(orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetOrgUnitsManagers(string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetOrgUnitsManagers(cultureName);
            return result;
        }
        public static async Task<GetResult<List<ActionDTO>>> GetOrgUnitActions(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetOrgUnitActions(orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TransactionAssignmentDTO>>> GetOrgUnitBeneficiaries(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetOrgUnitBeneficiaries(orgUnitId, cultureName);
            return result;
        }
        public static async Task<PostResult> LogTransactionAction(AuditingActionCode auditingActionCode, int transactionId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.LogTransactionAction(auditingActionCode, transactionId);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> GetInboundTransaction(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetInboundTransaction(transactionId, cultureName);
            return result;
        }
        public static async Task<PostObjectResult<TransactionDetailsDTO>> PostTransactionDraft(int transactionId, TransactionDTO transactionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostTransactionDraft(transactionId, transactionDTO);
            return result;
        }
        public static async Task<PutResult> PutTransaction(TransactionDTO transactionDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PutTransaction(transactionDTO);
            return result;
        }
        public static async Task<PutResult> PutTransactionBasicInfo(int transactionId, TransactionBasicInfoDTO transactionBasicInfoDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PutTransactionBasicInfo(transactionId, transactionBasicInfoDTO);
            return result;
        }
        public static async Task<PutResult> PostAssignTransaction(int transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.PostAssignTransaction(transactionId, transactionAssignmentDTOs);
            return result;
        }
        public static async Task<PostResult> AddTransactionExplanation(int transactionId, ExplanationDTO explanationDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.AddTransactionExplanation(transactionId, explanationDTO);
            return result;
        }
        public static async Task<PostResult> UpdateExplanation(ExplanationDTO explanationDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.UpdateExplanation(explanationDTO);
            return result;
        }
        public static async Task<GetResult<List<ExplanationDTO>>> GetTransactionExplanations(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionExplanations(transactionId, cultureName);
            return result;
        }
        public static async Task<GetResult<ExplanationDTO>> GetExplanationById(int explanationId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetExplanationById(explanationId, cultureName);
            return result;
        }
        public static async Task<PostResult> DeleteExplanation(int explanationId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.DeleteExplanation(explanationId);
            return result;
        }
        public static async Task<GetResult<DocumentDTO>> GetMainDocument(int transactionId)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetMainDocument(transactionId);
            return result;
        }
        public static async Task<PostResult> UpdateMainDocument(int transactionId, DocumentDTO documentDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.UpdateMainDocument(transactionId, documentDTO);
            return result;
        }
        public static async Task<PostResult> AddTransactionLinks(int transactionId, List<TransactionLinkDTO> transactionLinkDTOs)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.AddTransactionLinks(transactionId, transactionLinkDTOs);
            return result;
        }
        public static async Task<GetResult<TransactionBasicInfoDTO>> GetTransactionBasicInfo(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionBasicInfo(transactionId, cultureName);
            return result;
        }
        public static async Task<GetResult<TransactionDTO>> GetTransaction(int transactionId, int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransaction(transactionId, orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TransactionLinkDTO>>> GetTransactionLinks(int transactionId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetTransactionLinks(transactionId, cultureName);
            return result;
        }
        public static async Task<PutResult> UpdateAssignmentPaper(AssignmentPaperDTO assignmentPaperDTO)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.UpdateAssignmentPaper(assignmentPaperDTO);
            return result;
        }
        public static async Task<GetResult<AssignmentPaperDTO>> GetAssignmentPaperByOrgUnitId(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetAssignmentPaperByOrgUnitId(orgUnitId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<SubjectClassificationDTO>>> GetSubjectClassificationsByOrgUnitId(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetSubjectClassificationsByOrgUnitId(orgUnitId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<SuggestedTopicDTO>>> GetSuggestedTopicsByOrgUnitId(int orgUnitId, string cultureName)
        {
            var client = new TransactionApiHelper();
            var result = await client.TransactionApiClient.GetSuggestedTopicsByOrgUnitId(orgUnitId, cultureName);
            return result;
        }
    }
}