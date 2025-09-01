using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public class TransactionTaskBL : BaseBL, ITransactionTaskBL
    {
        public int GetTaskCount(int transactonId)
        {
            try
            {
                int Completedtask = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, string.Empty); 
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                return transactionTaskRepository.GetTasks(t => t.Transaction.Id == transactonId && t.Status.Id != Completedtask).Count;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public Task GetTask(int taskId, int transactonId, int orgUnitId)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                DateTime currDateTime = DateTime.Now.Date.AddDays(-1).AddMinutes(1);

                var result = transactionTaskRepository.GetTaskByIdAndorgUnitId(t => t.Id == taskId &&
                t.ToOrgUnitId == orgUnitId &&
                t.TransactionId == transactonId &&
                t.Parent == null &&
                t.IsDeleted == false &&
                currDateTime <= t.DeliveryDate);

                if (result == null)
                {
                    throw new BusinessException(StatusCode.TaskNotFound);
                }
                return result;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void MoveUserTasks(int transactonId, int userId)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

                IUserManagementBL userManagementBL = new UserManagementBL();

                IList<Task> tasks = transactionTaskRepository.GetTasks(ts => ts.ToUser.Id == userId && ts.Status.Id == TaskStatus.InProcess.LookupIdentity(LookupCategory.TaskStatus, string.Empty) && ts.Transaction.Id == transactonId);

                foreach (Task task in tasks)
                {
                    task.ToUser = userManagementBL.GetUserById(User.Id);
                    transactionTaskRepository.UpdateTask(task);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private int AddTask(int transactionId, Task task, string cultureName)
        {
            try
            {

                PreAddTask(transactionId, task);

                OnAddTask(task);

                PostAddTask(task);

                SendTaskNotification(task, NotificationSource.NewTask, NotificationTemplateType.NewTaskWeb, NotificationTemplateType.NewTaskEmail,
                    NotificationEmailSubject.NewTaskEmail, NotificationWebSubject.NewTask, cultureName);

                return task.Id;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AddTasks(int transactionId, IList<Task> tasks, string cultureName)
        {
            try
            {
                foreach (Task task in tasks)
                {
                    Task OldTask = GetTaskById(task.Id);
                    if (OldTask != null)
                    {
                        UpdateTask(task, cultureName);
                    }
                    else
                    {
                        AddTask(transactionId, task, cultureName);
                    }
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AddSubTask(int transactionId, IList<Task> tasks, string cultureName)
        {
            try
            {
                // PreAddSubTask(tasks);

                OnAddSubTask(transactionId, tasks, cultureName);

                PostAddSubTask(tasks);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateTask(Task task, string cultureName)
        {
            try
            {
                PreUpdateTask(task);

                OnUpdateTask(task);

                PostUpdateTask(task);

                SendTaskNotification(task, NotificationSource.ResendTask, NotificationTemplateType.ResendTaskWeb, NotificationTemplateType.ResendTaskEmail,
                NotificationEmailSubject.ResendTaskEmail, NotificationWebSubject.ResendTask, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void RejectTask(Task task)
        {
            try
            {
                PreRejectTask(task);

                OnRejectTask(task);

                PostRejectTask(task);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetReceivedTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, int ReceivedTasksTypeId, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                DateTime currDateTime = DateTime.Now.Date.AddDays(-1).AddMinutes(1);
                int Reject = TaskStatus.Reject.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                int Complete = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                int Sent = TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                if (ReceivedTasksTypeId == (int)ReceivedTasksType.AcceptedTasks)
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                          t.ToUser.Id == User.Id &&
                          t.ParentId == null &&
                          t.Status.Id != Reject &&
                          t.IsDeleted != true &&
                          //t.Status.Id != (int)TaskStatus.Sent &&
                          t.Status.Id != Complete, pageIndex, pageSize, cultureName, out rowsCount);
                }
                else if (ReceivedTasksTypeId == (int)ReceivedTasksType.NewTasks)
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                         t.ToUser.Id == User.Id &&
                         t.ParentId == null &&
                         t.IsDeleted != true &&
                         t.Status.Id == Sent &
                         currDateTime <= t.DeliveryDate,
                         pageIndex, pageSize, cultureName, out rowsCount);
                }
                else if (ReceivedTasksTypeId == (int)ReceivedTasksType.EndTasks)
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                         t.ToUser.Id == User.Id &&
                         t.IsDeleted != true &&
                         t.ParentId == null &&
                         t.Status.Id == Complete, //&
                         //currDateTime <= t.DeliveryDate,
                         pageIndex, pageSize, cultureName, out rowsCount);
                }
                else
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                         t.ToUser.Id == User.Id &&
                         t.IsDeleted != true &&
                         t.ParentId == null &&
                         t.Status.Id != Reject &&
                         t.Status.Id != Complete, pageIndex, pageSize, cultureName, out rowsCount);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetReceivedTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, SearchCriteriaCustom searchCriteria, int ReceivedTasksTypeId, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                int Reject = TaskStatus.Reject.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                int Complete = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                int Sent = TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, cultureName);

                if (ReceivedTasksTypeId == (int)ReceivedTasksType.AcceptedTasks)
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                          t.ToUser.Id == User.Id &&
                          t.ParentId == null &&
                          t.Status.Id != Reject &&
                          t.Status.Id != Sent &&
                          t.Status.Id != Complete, pageIndex, pageSize, cultureName, searchCriteria, out rowsCount);
                }
                else if (ReceivedTasksTypeId == (int)ReceivedTasksType.NewTasks)
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                         t.ToUser.Id == User.Id &&
                         t.ParentId == null &&
                         t.Status.Id == Sent,
                         pageIndex, pageSize, cultureName, searchCriteria, out rowsCount);
                }
                else
                {
                    return transactionTaskRepository.GetTasks(t => t.ToOrgUnit.Id == OrgUnitId &&
                         t.ToUser.Id == User.Id &&
                         t.ParentId == null &&
                         t.Status.Id != Reject &&
                         t.Status.Id != Complete, pageIndex, pageSize, cultureName, searchCriteria, out rowsCount);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetSentTasks(int pageIndex, int pageSize, int OrgUnitId, string cultureName, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

                IList<Task> tasks = transactionTaskRepository.GetTasks(t => t.FromOrgUnit.Id == OrgUnitId &&
                    t.FromUser.Id == User.Id, pageIndex, pageSize, cultureName, out rowsCount);

                ILookupBL lookupBL = new LookupBL();

                foreach (Task task in tasks)
                {
                    if (task.DeliveryDate < DateTime.Now && task.Status.Id == TaskStatus.InProcess.LookupIdentity(LookupCategory.TaskStatus, cultureName))
                    {
                        task.Status = lookupBL.GetLookupItem(TaskStatus.Late.LookupIdentity(LookupCategory.TaskStatus, cultureName));
                    }
                }

                return tasks;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetTasksByTransactionId(int transactionId, int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

                return transactionTaskRepository.GetTasks(t => t.ToUser.Id == User.Id &&
                    t.Transaction.Id == transactionId, pageIndex, pageSize, cultureName, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetTasks(Expression<Func<Task, bool>> @where)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                return transactionTaskRepository.GetTasks(@where);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Task GetTaskById(int taskId)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                return transactionTaskRepository.Get(taskId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        //public IList<OrgUnit> GetTaskSequenceOrgUnits(int taskId, int OrgUnitId, string cultureName)
        //{
        //    try
        //    {
        //        ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

        //        IList<OrgUnit> OrgUnits = new List<OrgUnit>();

        //        IUserManagementBL userManagementBL = new UserManagementBL();

        //        Task task = transactionTaskRepository.Get(taskId);

        //        UserProfile userProfile = userManagementBL.GetUserById(User.Id);

        //        OrgUnit OrgUnit = userProfile.OrgUnits.Where(o => o.Id == OrgUnitId).FirstOrDefault();

        //        IList<OrgUnitLink> OrgUnitsLinks = OrgUnit.Links;

        //        IList<TaskWorkflow> taskWorkflows = task.TaskWorkflows;

        //        IOrgUnitBL OrgUnitBL = new OrgUnitBL();

        //        foreach (TaskWorkflow taskWorkflow in taskWorkflows)
        //        {
        //            if (taskWorkflow.FromEntity.Id == OrgUnit.Id &&
        //                OrgUnitsLinks.Where(o => o.ToEntity.Id == taskWorkflow.ToEntity.Id).FirstOrDefault() != null)
        //            {
        //                OrgUnits.Add(taskWorkflow.ToEntity);
        //            }
        //        }

        //        OrgUnit userOrgUnit = OrgUnits.Where(o => o.Id == OrgUnitId).FirstOrDefault();

        //        if (userOrgUnit == null)
        //        {
        //            OrgUnits.Add(OrgUnit);
        //        }

        //        return OrgUnits;
        //    }
        //    catch (BusinessException)
        //    {
        //        throw;
        //    }
        //    catch (DataAccessException)
        //    {
        //        throw new BusinessException(StatusCode.GeneralError);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw BusinessException.Translate(ex);
        //    }
        //}

        //public IList<UserProfile> GetTaskSequenceUsers(int taskId, int fromOrgUnitId, int toOrgUnitId, string cultureName)
        //{
        //    try
        //    {
        //        ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

        //        IList<UserProfile> userProfiles = null;

        //        IUserManagementBL userManagementBL = new UserManagementBL();

        //        Task task = transactionTaskRepository.Get(taskId);

        //        if (task.TaskWorkflows != null)
        //        {
        //            TaskWorkflow taskWorkflow = task.TaskWorkflows.Where(tw => tw.FromEntity.Id == fromOrgUnitId &&
        //                tw.ToEntity.Id == toOrgUnitId).FirstOrDefault();

        //            if (taskWorkflow != null && taskWorkflow.ToUser != null)
        //            {
        //                userProfiles = new List<UserProfile>();

        //                userProfiles.Add(taskWorkflow.ToUser);

        //                return userProfiles;
        //            }
        //        }

        //        return userManagementBL.GetUsersByOrgUnitId(fromOrgUnitId, cultureName);
        //    }
        //    catch (BusinessException)
        //    {
        //        throw;
        //    }
        //    catch (DataAccessException)
        //    {
        //        throw new BusinessException(StatusCode.GeneralError);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw BusinessException.Translate(ex);
        //    }
        //}

        public void ExtendTaskDate(int taskId, DateTime dateTime)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

                Task task = transactionTaskRepository.Get(taskId);

                if (task.DeliveryDate >= dateTime)
                {
                    throw new BusinessException(StatusCode.InvalidExtendTaskDate);
                }

                task.DeliveryDate = dateTime;
                task.DeliveryDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(dateTime);

                transactionTaskRepository.UpdateTask(task);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AcceptTask(int taskId, int taskAcceptanceStatus, string RejectionReason, string cultureName)
        {
            try
            {
                Task task = GetTaskById(taskId);

                if (taskAcceptanceStatus == (int)TaskAcceptanceStatus.Accept)
                {
                    task.StatusId = TaskStatus.Received.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                    SendTaskNotification(task, NotificationSource.AcceptTask, NotificationTemplateType.AcceptTaskWeb,
                        NotificationTemplateType.AcceptTaskEmail, NotificationEmailSubject.AcceptTaskEmail, NotificationWebSubject.AcceptTask, cultureName);
                }
                else
                {
                    task.StatusDescription = RejectionReason.Trim();
                    task.StatusId = TaskStatus.Reject.LookupIdentity(LookupCategory.TaskStatus, cultureName);
                    SendTaskNotification(task, NotificationSource.RejectTask, NotificationTemplateType.RejectTaskWeb,
                        NotificationTemplateType.RejectTaskEmail, NotificationEmailSubject.RejectTaskEmail, NotificationWebSubject.RejectTask, cultureName);
                }

                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                transactionTaskRepository.UpdateTask(task);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void ResendTask(int taskId, string ResendReason, int ExpectedDays, string cultureName)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                transactionTaskRepository.ResendTask(taskId, ResendReason, ExpectedDays);

                Task task = GetTaskById(taskId);
                SendTaskNotification(task, NotificationSource.ResendTask, NotificationTemplateType.ResendTaskWeb,
                    NotificationTemplateType.ResendTaskEmail, NotificationEmailSubject.ResendTaskEmail, NotificationWebSubject.ResendTask, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void CompleteTask(Task task, string cultureName)
        {
            try
            {
                PreCompleteTask(task);

                OnCompleteTask(task);

                PostCompleteTask(task);

                SendTaskNotification(task, NotificationSource.ReplyTask, NotificationTemplateType.ReplyTaskWeb,
                   NotificationTemplateType.ResendTaskEmail, NotificationEmailSubject.ResendTaskEmail, NotificationWebSubject.ResendTask, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<TasksAttachments> GetTaskAttachments(int TaskId)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            return transactionTaskRepository.GetTaskAttachments(TaskId);
        }
        public DocumentInfo GetDocumentInfoById(int DocumentInfoId)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            return transactionTaskRepository.GetDocumentInfoById(DocumentInfoId);
        }

        public void SendTaskReminder(int taskId, string cultureName)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

                Task task = transactionTaskRepository.Get(taskId);
                PreSendTaskReminder(task);

                OnSendTaskReminder(task);

                PostSendTaskReminder(task);

                SendTaskNotification(task, NotificationSource.TaskReminder, NotificationTemplateType.TaskReminderWeb,
                    NotificationTemplateType.TaskReminderEmail, NotificationEmailSubject.TaskReminderEmail, NotificationWebSubject.TaskReminder, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Task> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                return transactionTaskRepository.GetTransactionTasks(transactionId, searchCriteria, cultureName, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<Task> GetTransactionTasksReply(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                return transactionTaskRepository.GetTransactionTasksReply(transactionId, searchCriteria, cultureName, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void CheckEndTasks(string cultureName)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                var tasks = transactionTaskRepository.CheckEndTasks();
                if (tasks != null && tasks.Count > 0)
                {
                    foreach (var task in tasks)
                    {
                        SendTaskNotification(task, NotificationSource.DeleteTask, NotificationTemplateType.DeleteTaskWeb,
                        NotificationTemplateType.DeleteTaskEmail, NotificationEmailSubject.DeleteTaskEmail, NotificationWebSubject.DeleteTask, cultureName);
                    }
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private void PreAddTask(int transactionId, Task task)
        {
            task.CreatedOn = DateTime.UtcNow;
            if (!User.HasClaim(UserClaims.Tasks.Add))
            {
                throw new BusinessException(StatusCode.PermissionTasksInsertionAddTask);
            }

            Transaction transaction = TransactionBL.StaticGetTransaction(t => t.Id == transactionId);
            ILookupBL lookupBL = new LookupBL();

            task.StatusId = TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
            task.FromUserId = User.Id;
            task.Date = DateTime.Now;
            task.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            task.DeliveryDateH = task.DeliveryDateH;// DateTimeUtility.ConvertToUmAlQuraCalendar(task.DeliveryDate);
            task.Transaction = transaction ?? throw new BusinessException(StatusCode.TransactionNotFound);
        }

        private void OnAddTask(Task task)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            transactionTaskRepository.AddTask(task);
        }

        private void PostAddTask(Task task)
        {
            //  ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            ITaskHistoryRepository taskHistoryRepository = IoC.Resolve<ITaskHistoryRepository>();
            //if (!task.IsExclusive & task.TaskWorkflows != null)
            //{
            //    transactionTaskRepository.AddTaskWorkflow(task.Id, task.TaskWorkflows);
            //}
            taskHistoryRepository.AddTaskHistory(GetTaskHistory(task));
        }

        private void PreUpdateTask(Task task)
        {
            if (!User.HasClaim(UserClaims.Tasks.Edit))
            {
                throw new BusinessException(StatusCode.PermissionTasksInsertionEditTaskDate);
            }
        }

        private void OnUpdateTask(Task task)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();

            transactionTaskRepository.UpdateTask(task);
        }

        private void PostUpdateTask(Task task)
        {
        }

        private void PreDeleteTasks(IList<int> ids)
        {
            if (!User.HasClaim(UserClaims.Tasks.Delete))
            {
                throw new BusinessException(StatusCode.PermissionTasksInsertionDeleteTask);
            }
        }

        private void OnDeleteTasks(IList<int> ids)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            foreach (int id in ids)
            {
                transactionTaskRepository.DeleteTask(id);
            }
        }

        private void PostDeleteTasks(IList<int> ids)
        {
        }

        public void DeleteTasks(IList<int> ids, string cultureName)
        {
            List<Task> tasks = new List<Task>();
            foreach (var item in ids)
            {
                var taskToSendNotification = GetTaskById(item);
                tasks.Add(taskToSendNotification);
            }

            PreDeleteTasks(ids);

            OnDeleteTasks(ids);

            PostDeleteTasks(ids);

            foreach (var task in tasks)
            {
                SendTaskNotification(task, NotificationSource.DeleteTask, NotificationTemplateType.DeleteTaskWeb,
                    NotificationTemplateType.DeleteTaskEmail, NotificationEmailSubject.DeleteTaskEmail, NotificationWebSubject.DeleteTask, cultureName);
            }
        }
        private void PreRejectTask(Task task)
        {
            if (User.Id != task.ToUser.Id)
            {
                throw new BusinessException(StatusCode.InvalidUserTask);
            }

            ILookupBL lookupBL = new LookupBL();

            task.Status = lookupBL.GetLookupItem(TaskStatus.Reject.LookupIdentity(LookupCategory.TaskStatus, string.Empty));
            task.FromUser = task.FromUser;
            task.ToUser = task.ToUser;
        }

        private void OnRejectTask(Task task)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            transactionTaskRepository.UpdateTask(task);
        }

        private void PostRejectTask(Task task)
        {
            ITaskHistoryRepository taskHistoryRepository = IoC.Resolve<ITaskHistoryRepository>();
            taskHistoryRepository.AddTaskHistory(GetTaskHistory(task));
        }

        private void PreCompleteTask(Task task)
        {
            if (User.Id != task.ToUser.Id)
            {
                throw new BusinessException(StatusCode.InvalidUserTask);
            }

            ILookupBL lookupBL = new LookupBL();

            // task.Status = lookupBL.GetLookupItem((int)TaskStatus.Complete);

        }

        private void OnCompleteTask(Task task)
        {
            ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
            //transactionTaskRepository.UpdateTask(task);
            transactionTaskRepository.CompleteTask(task);
        }

        private void PostCompleteTask(Task task)
        {
            ITaskHistoryRepository taskHistoryRepository = IoC.Resolve<ITaskHistoryRepository>();
            taskHistoryRepository.AddTaskHistory(GetTaskHistory(task));
        }

        //private void PreAddSubTask(IList<Task> tasks)
        //{
        //    if (!User.HasClaim(UserClaims.Tasks.Sub))
        //    {
        //        throw new BusinessException(StatusCode.PermissionTasksInsertionSubTasking);
        //    }

        //    int taskLimitationLevel = SystemConfigurations.TaskLimitationLevel;

        //    foreach (Task task in tasks)
        //    {
        //        if (task.Parent == null)
        //        {
        //            throw new BusinessException(StatusCode.InvalidParentTask);
        //        }

        //        if (task.Parent.LevelLimitation > taskLimitationLevel)
        //        {
        //            throw new BusinessException(StatusCode.LimitaionSubTask);
        //        }

        //        TaskWorkflow taskWorkflow = task.Parent.TaskWorkflows.Where(tw => tw.FromEntity.Id == task.FromOrgUnit.Id &
        //            tw.ToEntity.Id == task.ToOrgUnit.Id).FirstOrDefault();

        //        if (taskWorkflow == null)
        //        {
        //            throw new BusinessException(StatusCode.InvalidTaskWorkflow);
        //        }

        //        task.TransactionAssignment = task.Parent.TransactionAssignment;
        //        task.IsExclusive = true;
        //        task.TaskWorkflows = null;
        //        task.LevelLimitation = taskLimitationLevel + 1;
        //    }
        //}

        private void OnAddSubTask(int transactionId, IList<Task> tasks, string cultureName)
        {
            foreach (Task task in tasks)
            {
                if (task.Parent != null)
                {
                    AddTask(transactionId, task, cultureName);
                }
            }
        }

        private void PostAddSubTask(IList<Task> tasks)
        {
            ITaskHistoryRepository taskHistoryRepository = IoC.Resolve<ITaskHistoryRepository>();
            foreach (Task task in tasks)
            {
                taskHistoryRepository.AddTaskHistory(GetTaskHistory(task));
            }
        }

        private void PreSendTaskReminder(Task task)
        {
            if (task == null)
            {
                throw new BusinessException(StatusCode.InvalidTask);
            }

            if (!User.HasClaim(UserClaims.Tasks.Reminder))
            {
                throw new BusinessException(StatusCode.PermissionTasksInsertionTaskReminder);
            }

            if (task.Status.Id == TaskStatus.Reject.LookupIdentity(LookupCategory.TaskStatus, string.Empty))
            {
                throw new BusinessException(StatusCode.TaskReminderNotAllow);
            }
        }

        private void OnSendTaskReminder(Task task)
        {
            ITaskReminderRepository taskReminderRepository = IoC.Resolve<ITaskReminderRepository>();

            TaskReminder taskReminder = new TaskReminder();

            string umAlQuraDate = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

            taskReminder.Task = task;
            taskReminder.Date = DateTime.Now;
            taskReminder.DateH = umAlQuraDate;

            taskReminderRepository.AddTaskReminder(taskReminder);
        }

        private void PostSendTaskReminder(Task task)
        {
        }

        private TaskHistory GetTaskHistory(Task task)
        {
            TaskHistory result = new TaskHistory()
            {
                Date = task.Date,
                DateH = task.DateH,
                DeliveryDate = task.DeliveryDate,
                DeliveryDateH = task.DeliveryDateH,
                FromOrgUnit = task.FromOrgUnit,
                FromUser = task.FromUser,
                IsExclusive = task.IsExclusive,
                Parent = task.Parent,
                Status = task.Status,
                StatusDescription = task.StatusDescription,
                TaskDescription = task.TaskDescription,
                ToOrgUnit = task.ToOrgUnit,
                ToUser = task.ToUser
            };
            return result;
        }

        private void SendTaskNotification(Task task, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            string cultureName, string taskProcessingPeriod = "")
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                int userId;
                IList<NotificationUser> notificationUsers = new List<NotificationUser>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                if (notificationTemplateType == NotificationTemplateType.AcceptTaskWeb ||
                    notificationTemplateType == NotificationTemplateType.RejectTaskWeb)
                {
                    notificationUsers.Add(NotificationsManager.BuildNotificationUser(task.FromUserId));
                    userId = task.FromUserId;
                }
                else
                {
                    notificationUsers.Add(NotificationsManager.BuildNotificationUser(task.ToUserId));
                    userId = task.ToUserId;
                }
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId, cultureName);
                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Tasks))
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    var transaction = TransactionBL.GetTransactionById(task.TransactionId, cultureName, true);
                    keyValues.Add("{TaskId}", task.Id.ToString());
                    keyValues.Add("{TransactionId}", transaction.Id.ToString());
                    keyValues.Add("{Number}", transaction.Number.ToString());
                    keyValues.Add("{Subject}", transaction.Subject);
                    keyValues.Add("{TransactionTypeId}", transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text);
                    keyValues.Add("{PriorityId}", transaction.Priority.Text);
                    keyValues.Add("{ConfidentialityId}", transaction.Confidentiality.LocalName);
                    keyValues.Add("{UserName}", User?.UserName);
                    keyValues.Add("{DeliveryDateH}", task.DeliveryDateH);
                    keyValues.Add("{StatusDescription}", task.StatusDescription);
                    keyValues.Add("{TaskProcessingPeriod}", taskProcessingPeriod);

                    //Notification Web
                  //  NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                    //Notification Email
                    if (SystemConfigurations.MultiTenantEnabled)
                    {
                        TenantBL tenantBL = new TenantBL();
                        tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType, notificationEmailSubject,
                            notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                    }
                    else
                    {
                        var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                        //System Notification  Email
                        NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                            notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                    }
                }
            }
        }

        public void SendToUserReminderBeforeTaskEnded(int taskProcessingPeriod, int taskReminderCount, string cultureName)
        {
            try
            {
                ITransactionTaskRepository transactionTaskRepository = IoC.Resolve<TransactionTaskRepository>();
                List<int> taskIds = new List<int>();
                var tasks = transactionTaskRepository.SendToUserReminderBeforeTaskEnded(taskProcessingPeriod, taskReminderCount);
                foreach (var task in tasks)
                {
                    if (task.NumberOfNotifications < taskReminderCount)
                    {
                        SendTaskNotification(task, NotificationSource.ReminderBeforeTaskEnded, NotificationTemplateType.ReminderBeforeTaskEndedWeb,
                            NotificationTemplateType.ReminderBeforeTaskEndedEmail, NotificationEmailSubject.ReminderBeforeTaskEndedEmail,
                            NotificationWebSubject.ReminderBeforeTaskEnded, cultureName, taskProcessingPeriod.ToString());
                        taskIds.Add(task.Id);
                    }
                }
                if (taskIds.Count > 0)
                {
                    transactionTaskRepository.UpdateTaskReminderBeforeEnded(taskIds);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
