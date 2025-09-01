using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface ITransactionTaskRepository : IRepository<Task>
    {

        int AddTask(Task task);
        void UpdateTask(Task task);
        void ResendTask(int taskId, string resendReason, int ExpectedDays);
        void CompleteTask(Task task);
        void DeleteTask(int id);
        void AddTaskWorkflow(int taskId, IList<TaskWorkflow> taskWorkflows);
        void DeleteTaskWorkflow(int taskId);
        IList<Task> CheckEndTasks();
        DocumentInfo GetDocumentInfoById(int DocumentInfoId);
        IList<TasksAttachments> GetTaskAttachments(int TaskId);
        IList<Task> GetTasks(Expression<Func<Task, bool>> @where, int pageIndex, int pageSize, string cultureName, out int rowsCount);
        IList<Task> GetTasks(Expression<Func<Task, bool>> @where, int pageIndex, int pageSize, string cultureName, SearchCriteriaCustom searchCriteria, out int rowsCount);
        IList<Task> GetTasks(Expression<Func<Task, bool>> @where);
        Task GetTaskByIdAndorgUnitId(Expression<Func<Task, bool>> @where);
        Task GetTaskById(int taskId);
        IList<Task> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        IList<Task> GetTransactionTasksReply(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount);
        IList<Task> SendToUserReminderBeforeTaskEnded(int taskProcessingPeriod, int taskReminderCount);
        void UpdateTaskReminderBeforeEnded(List<int> ids);
    }
}
