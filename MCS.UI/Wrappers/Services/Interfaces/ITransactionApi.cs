using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.UI.Wrappers
{
    public interface ITransactionApi
    {
        [Get("/api/transaction/GetNotifications")]
        Task<GetResult<List<NotificationDTO>>> GetNotifications(SearchCriteria searchCriteria, string cultureName);

        [Get("/api/transaction/DeleteNotifications")]
        Task<DeleteResult> DeleteNotifications(string ids);

        [Get("/api/transaction/PostTransaction")]
        Task<PostObjectResult<TransactionDetailsDTO>> PostTransaction(TransactionDTO transactionDTO, string cultureName);

        [Get("/api/transaction/PutTransaction")]
        Task<PutResult> PutTransaction(string cultureName, TransactionDTO transactionDTO);

        [Get("/api/transaction/PrintTransactionTicket")]
        Task<GetResult<TransactionTicketDTO>> PrintTransactionTicket(TransactionTicketInfoDTO transactionTicketInfoDTO, string cultureName);

        [Get("/api/transaction/GetTransactionBarcodes")]
        Task<GetResult<TransactionBarcodesDTO>> GetTransactionBarcodes(int transactionId, int orgUnitId, string cultureName);

        [Get("/api/transaction/PrintDeliveryReportById")]
        Task<GetResult<List<DeliveryReportDTO>>> PrintDeliveryReportById(string strTransactionReportInfos, string cultureName, bool perTransaction = true);

        [Get("/api/transaction/PrintDeliveryReport")]
        Task<GetResult<List<DeliveryReportDTO>>> PrintDeliveryReport(int transactionId, string cultureName, bool perTransaction = true);

        [Get("/api/transaction/GetTransactionCertificateById")]
        Task<GetResultExtraData<object>> GetTransactionCertificateById(int transactionId, string cultureName);

        [Get("/api/transaction/GetTransactionCertificateByReference")]
        Task<GetResultExtraData<object>> GetTransactionCertificateByReference(string referenceCode, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetInboundCertificate")]
        Task<GetResult<InboundCertificateDTO>> GetInboundCertificate(int transactionId, string cultureName);

        [Get("/api/transaction/GetOutboundCertificate")]
        Task<GetResult<OutboundCertificateDTO>> GetOutboundCertificate(int transactionId, string cultureName);

        [Get("/api/transaction/SearchDeliveryReport")]
        Task<GetResult<List<TransactionDeliveryReportDTO>>> SearchDeliveryReport(string strSearchCriteria);

        [Get("/api/transaction/GetTransactionVisitTicket")]
        Task<GetResult<TransactionVisitTicketDTO>> GetTransactionVisitTicket(int transactionId, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetTransactionName")]
        Task<GetResult<TransactionNameDTO>> GetTransactionName(string civilID, string cultureName);

        [Get("/api/transaction/GetTransaction")]
        Task<GetResult<TransactionDTO>> GetTransaction(int userId, int transactionNumber, TransactionCategory transactionType, int year, int sourceId, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetTransactionIdByLinkType")]
        Task<GetResult<int>> GetTransactionIdByLinkType(int linkTypeId, string sourceNumber, int orgUnitId, int yearId, string cultureName);

        [Get("/api/transaction/GetPreviousTransaction")]
        Task<GetResult<TransactionDTO>> GetPreviousTransaction(TransactionCategory transactionType, int orgUnitId, string cultureName);

        [Get("/api/transaction/PostTransactionAssignment")]
        Task<PostResult> PostTransactionAssignment(List<int> transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string cultureName);

        [Get("/api/transaction/PostTransactionTasks")]
        Task<PostResult> PostTransactionTasks(TransactionTaskDTO transactionTaskDTOs, string cultureName);

        [Get("/api/transaction/PutRejectTransactionTask")]
        Task<PutResult> PutRejectTransactionTask(TaskActionDTO taskActionDTO);

        [Get("/api/transaction/PutCompleteTransactionTask")]
        Task<PutResult> PutCompleteTransactionTask(TaskActionDTO taskActionDTO);

        [Get("/api/transaction/PostSubTransactionTask")]
        Task<PostResult> PostSubTransactionTask(TransactionSubTaskDTO transactionSubTaskDTO, string cultureName);

        [Get("/api/transaction/GetTasksCount")]
        Task<GetResult<int?>> GetTasksCount(int assignmentId);

        [Get("/api/transaction/GetReceivedTasks")]
        Task<GetResult<List<ReceivedTaskDTO>>> GetReceivedTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetUserTasksStatus")]
        Task<GetResult<List<TaskStatusDTO>>> GetUserTasksStatus(int userId, int orgUnitId);

        [Get("/api/transaction/GetReceivedTask")]
        Task<GetResult<ReceivedTaskDTO>> GetReceivedTask(int taskId, string cultureName);

        [Get("/api/transaction/GetSentTask")]
        Task<GetResult<SentTaskDTO>> GetSentTask(int taskId, string cultureName);

        [Get("/api/transaction/GetSentTasks")]
        Task<GetResult<List<SentTaskDTO>>> GetSentTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName);

        [Get("/api/transaction/PostTaskReminder")]
        Task<PostResult> PostTaskReminder(int taskId, string cultureName);

        [Get("/api/transaction/GetTaskSequenceOrgUnits")]
        Task<GetResult<List<OrgUnitDTO>>> GetTaskSequenceOrgUnits(int taskId, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetTaskSequenceUsers")]
        Task<GetResult<List<UserProfileDTO>>> GetTaskSequenceUsers(int taskId, int fromOrgUnitId, int toOrgUnitId, string cultureName);

        [Get("/api/transaction/ExtendTaskDate")]
        Task<PostResult> ExtendTaskDate(int taskId, string dateTime);

        [Get("/api/transaction/GetTransactionTasks")]
        Task<GetResult<List<TaskAddDTO>>> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName);

        [Get("/api/transaction/GetUserTrays")]
        Task<GetResult<List<TrayDetailsDTO>>> GetUserTrays(int orgUnitId, string cultureName);

        [Get("/api/transaction/GetTrayDetailsInfo")]
        Task<GetResult<TrayDetailsDTO>> GetTrayDetailsInfo(TrayType trayType, int orgUnitId, SearchCriteria searchCriteria);

        [Get("/api/transaction/GetPopulariazations")]
        Task<GetResult<TrayDetailsDTO>> GetPopulariazations(int orgUnitId, SearchCriteria searchCriteria);

        [Get("/api/transaction/GetUserTransactionsTray")]
        Task<GetResult<List<TransactionTrayInfoDTO>>> GetUserTransactionsTray(TrayType trayType, int orgUnitId, TransactionDateType transactionDate, SearchCriteria searchCriteria);

        [Get("/api/transaction/MoveTransaction")]
        Task<PutResult> MoveTransaction(int transactionId, int orgUnitId, int trayActionTypeId, int trayId, int? assigmentId, object extraParams);

        [Get("/api/transaction/CreateOutboundExternal")]
        Task<PostObjectResult<TransactionDetailsDTO>> CreateOutboundExternal(int transactionId, int trayId, TransactionDTO transactionDTO);

        [Get("/api/transaction/PrepareOutboundCreation")]
        Task<GetResult<TransactionDTO>> PrepareOutboundCreation(int transactionId, int orgUnitId, int trayId, string cultureName);

        [Get("/api/transaction/GetPriorities")]
        Task<GetResult<TransactionDTO>> GetPriorities(TransactionCategory transactionType, string cultureName);

        [Get("/api/transaction/GetTransactionTypes")]
        Task<GetResult<List<TransactionTypeDTO>>> GetTransactionTypes(TransactionCategory transactionType, string cultureName);

        [Get("/api/transaction/GetLinkTypes")]
        Task<GetResult<List<LinkDTO>>> GetLinkTypes(TransactionCategory transactionType, string cultureName);

        [Get("/api/transaction/GetContentByFormId")]
        Task<GetResult<FormContentDTO>> GetContentByFormId(int formId);

        [Get("/api/transaction/GetLetterTypes")]
        Task<GetResult<List<LetterTypeDTO>>> GetLetterTypes(TransactionCategory transactionType, string cultureName);

        [Get("/api/transaction/GetAttachmentTypes")]
        Task<GetResult<List<AttachmentTypeDTO>>> GetAttachmentTypes(TransactionCategory transactionType, string cultureName);

        [Get("/api/transaction/CheckOrgUnitHasAssignmentPaper")]
        Task<GetResult<bool>> CheckOrgUnitHasAssignmentPaper(int orgUnitId);

        [Get("/api/transaction/CheckOrgUnitIsAllowedToCreateGroup")]
        Task<GetResult<bool>> CheckOrgUnitIsAllowedToCreateGroup(int orgUnitId);

        [Get("/api/transaction/GetOrgUnitLinks")]
        Task<GetResult<List<OrgUnitDTO>>> GetOrgUnitLinks(int orgUnitId, string cultureName);

        [Get("/api/transaction/GetOrgUnitsManagers")]
        Task<GetResult<List<UserProfileDTO>>> GetOrgUnitsManagers(string cultureName);

        [Get("/api/transaction/GetOrgUnitActions")]
        Task<GetResult<List<ActionDTO>>> GetOrgUnitActions(int orgUnitId, string cultureName);

        [Get("/api/transaction/GetOrgUnitBeneficiaries")]
        Task<GetResult<List<TransactionAssignmentDTO>>> GetOrgUnitBeneficiaries(int orgUnitId, string cultureName);

        [Get("/api/transaction/LogTransactionAction")]
        Task<PostResult> LogTransactionAction(AuditingActionCode auditingActionCode, int transactionId);

        [Get("/api/transaction/GetInboundTransaction")]
        Task<GetResult<TransactionDTO>> GetInboundTransaction(int transactionId, string cultureName);

        [Get("/api/transaction/PostTransactionDraft")]
        Task<PostObjectResult<TransactionDetailsDTO>> PostTransactionDraft(int transactionId, TransactionDTO transactionDTO);

        [Get("/api/transaction/PutTransaction")]
        Task<PutResult> PutTransaction(TransactionDTO transactionDTO);

        [Get("/api/transaction/PutTransactionBasicInfo")]
        Task<PutResult> PutTransactionBasicInfo(int transactionId, TransactionBasicInfoDTO transactionBasicInfoDTO);

        [Get("/api/transaction/PostAssignTransaction")]
        Task<PutResult> PostAssignTransaction(int transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs);

        [Get("/api/transaction/AddTransactionExplanation")]
        Task<PostResult> AddTransactionExplanation(int transactionId, ExplanationDTO explanationDTO);

        [Get("/api/transaction/UpdateExplanation")]
        Task<PostResult> UpdateExplanation(ExplanationDTO explanationDTO);

        [Get("/api/transaction/GetTransactionExplanations")]
        Task<GetResult<List<ExplanationDTO>>> GetTransactionExplanations(int transactionId, string cultureName);

        [Get("/api/transaction/GetExplanationById")]
        Task<GetResult<ExplanationDTO>> GetExplanationById(int explanationId, string cultureName);

        [Get("/api/transaction/DeleteExplanation")]
        Task<PostResult> DeleteExplanation(int explanationId);

        [Get("/api/transaction/GetMainDocument")]
        Task<GetResult<DocumentDTO>> GetMainDocument(int transactionId);

        [Get("/api/transaction/UpdateMainDocument")]
        Task<PostResult> UpdateMainDocument(int transactionId, DocumentDTO documentDTO);

        [Get("/api/transaction/AddTransactionLinks")]
        Task<PostResult> AddTransactionLinks(int transactionId, List<TransactionLinkDTO> transactionLinkDTOs);

        [Get("/api/transaction/GetTransactionBasicInfo")]
        Task<GetResult<TransactionBasicInfoDTO>> GetTransactionBasicInfo(int transactionId, string cultureName);

        [Get("/api/transaction/GetTransaction")]
        Task<GetResult<TransactionDTO>> GetTransaction(int transactionId, int orgUnitId, string cultureName);

        [Get("/api/transaction/GetTransactionLinks")]
        Task<GetResult<List<TransactionLinkDTO>>> GetTransactionLinks(int transactionId, string cultureName);

        [Get("/api/transaction/UpdateAssignmentPaper")]
        Task<PutResult> UpdateAssignmentPaper(AssignmentPaperDTO assignmentPaperDTO);

        [Get("/api/transaction/GetAssignmentPaperByOrgUnitId")]
        Task<GetResult<AssignmentPaperDTO>> GetAssignmentPaperByOrgUnitId(int orgUnitId, string cultureName);

        [Get("/api/transaction/GetSubjectClassificationsByOrgUnitId")]
        Task<GetResult<List<SubjectClassificationDTO>>> GetSubjectClassificationsByOrgUnitId(int orgUnitId, string cultureName);

        [Get("/api/transaction/GetSuggestedTopicsByOrgUnitId")]
        Task<GetResult<List<SuggestedTopicDTO>>> GetSuggestedTopicsByOrgUnitId(int orgUnitId, string cultureName);



    }
}

