using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface ITransactionTaskBL
    {
        void AddTasks(int transactionId, IList<Task> tasks, string cultureName);
        // void AddSubTask(int transactionId, IList<Task> tasks, string cultureName);
        void UpdateTask(Task task, string cultureName);
        void DeleteTasks(IList<int> ids, string cultureName);
        void RejectTask(Task task);
        void CompleteTask(Task task, string cultureName);
        void AcceptTask(int taskId, int taskAcceptanceStatus, string RejectionReason, string cultureName);
        void ResendTask(int taskId, string resendReason, int ExpectedDays, string cultureName);
        Task GetTaskById(int taskId);
        void ExtendTaskDate(int taskId, DateTime dateTime);
        void SendTaskReminder(int taskId, string cultureName);
        void MoveUserTasks(int assignmentId, int fromUserId);
        int GetTaskCount(int assignmentId);
        DocumentInfo GetDocumentInfoById(int DocumentInfoId);
        IList<TasksAttachments> GetTaskAttachments(int TaskId);
        IList<Task> GetSentTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, out int rowsCount);
        IList<Task> GetReceivedTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, int ReceivedTasksTypeId, out int rowsCount);
        IList<Task> GetReceivedTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, SearchCriteriaCustom searchCriteria, int ReceivedTasksTypeId, out int rowsCount);
        //  IList<OrgUnit> GetTaskSequenceOrgUnits(int taskId, int OrgUnitId, string cultureName);
        //IList<UserProfile> GetTaskSequenceUsers(int taskId, int fromOrgUnitId, int toOrgUnitId, string cultureName);
        IList<Task> GetTasksByTransactionId(int transactionId, int pageIndex, int pageSize, string cultureName, out int rowsCount);
        IList<Task> GetTasks(Expression<Func<Task, bool>> @where);
        IList<Task> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        IList<Task> GetTransactionTasksReply(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        void CheckEndTasks(string cultureName);
        void SendToUserReminderBeforeTaskEnded(int taskProcessingPeriod, int taskReminderCount, string cultureName);
        Task GetTask(int taskId, int transactonId, int orgUnitId);
    }
}
