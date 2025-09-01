using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Common.Utility;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public class TransactionTaskRepository : BaseRepository<Task>, ITransactionTaskRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionTaskRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public int AddTask(Task task)
        {
            try
            {
                _oMCSDbContext.Tasks.Add(task);

                _oMCSDbContext.SaveChanges();

                return task.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void CompleteTask(Task task)
        {
            try
            {
                Task taskOld = GetTaskById(task.Id);

                task.Id = 0;
                // swap users & entities 
                task.ToUser = null;
                task.ToOrgUnit = null;
                task.FromOrgUnit = null;
                task.FromUser = null;

                task.ToOrgUnitId = taskOld.FromOrgUnitId;
                task.FromOrgUnitId = taskOld.ToOrgUnitId;
                task.FromUserId = taskOld.ToUserId;
                task.ToUserId = taskOld.FromUserId;

                task.ParentId = taskOld.Id;

                _oMCSDbContext.Tasks.Add(task);

                _oMCSDbContext.SaveChanges();

                taskOld.StatusId = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, string.Empty);

                _oMCSDbContext.Entry(taskOld).State = EntityState.Modified;

                _oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void UpdateTask(Task task)
        {
            try
            {
                Task taskOld = GetTaskById(task.Id);
                if (taskOld != null)
                {
                    task.TransactionId = taskOld.TransactionId;
                    taskOld.ToOrgUnitId = task.ToOrgUnitId;
                    taskOld.ToUserId = task.ToUserId;
                    taskOld.TaskDescription = task.TaskDescription;
                    taskOld.DeliveryDate = task.DeliveryDate;
                    taskOld.DeliveryDateH = task.DeliveryDateH;
                    if (task.StatusId != 0)
                    {
                        taskOld.StatusId = task.StatusId;
                    }

                    if (task.StatusDescription != null && task.StatusDescription != "")
                    {
                        taskOld.StatusDescription = task.StatusDescription;
                    }

                    _oMCSDbContext.Entry(taskOld).State = EntityState.Modified;

                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void ResendTask(int taskId, string resendReason, int ExpectedDays)
        {
            try
            {
                Task task = GetTaskById(taskId);

                task.StatusId = TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
                task.StatusDescription = resendReason;
                task.DeliveryDate = DateTime.Now.AddDays(ExpectedDays);
                task.DeliveryDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now.AddDays(ExpectedDays));
                //task.ToUserId = task.FromUserId;
                //task.ToOrgUnitId = task.FromOrgUnitId;

                _oMCSDbContext.Entry(task).State = EntityState.Modified;

                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                Task task = this.FindBy(p => p.Id == id);

                if (task != null)
                {
                    task.IsDeleted = true;
                }

                _oMCSDbContext.Entry(task).State = EntityState.Modified;
                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteTaskWorkflow(int taskId)
        {
            try
            {
                //Task task = GetTaskById(taskId);

                //if (task != null)
                //{
                //    task.TaskWorkflows.ToList().ForEach(item => _oMCSDbContext.TaskWorkflows.Remove(item));

                //    _oMCSDbContext.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void AddTaskWorkflow(int taskId, IList<TaskWorkflow> taskWorkflows)
        {
            try
            {
                //Task task = GetTaskById(taskId);

                //task.TaskWorkflows = taskWorkflows;

                //_oMCSDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Task> GetTasks(Expression<Func<Task, bool>> @where, int pageIndex, int pageSize, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<Task> tasks = _oMCSDbContext.Tasks.Include(a => a.Transaction).Where(@where).AsQueryable();

                rowsCount = tasks.Count();


                List<Task> Result = tasks.OrderByDescending(t => t.Id)
                              .Skip((pageIndex - 1) * pageSize)
                              .Take(pageSize).ToList();

                List<TasksAttachments> tasksAttachments = (from taskAtt in _oMCSDbContext.TasksAttachments
                                                           join task in _oMCSDbContext.Tasks on taskAtt.TaskId equals task.Id
                                                           select taskAtt).ToList();

                //Result.Select(r => r.TasksAttachments == tasksAttachments.Where(ta => ta.TaskId == r.Id)).ToList();


                foreach (var item in Result)
                {
                    var Attachments = tasksAttachments.Where(ta => ta.TaskId == item.Id).ToList();

                    item.TasksAttachments = Attachments;
                }


                return Result;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<TasksAttachments> GetTaskAttachments(int TaskId)
        {
            try
            {
                List<TasksAttachments> tasksAttachments = _oMCSDbContext.TasksAttachments.Where(ta => ta.TaskId == TaskId).ToList();

                return tasksAttachments;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public DocumentInfo GetDocumentInfoById(int documentInfoId)
        {
            try
            {
                DocumentInfo documentInfo = _oMCSDbContext.DocumentsInfo.Include(t => t.Document).Where(ta => ta.Id == documentInfoId).FirstOrDefault();

                return documentInfo;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }




        public IList<Task> GetTasks(Expression<Func<Task, bool>> @where, int pageIndex, int pageSize, string cultureName, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                IQueryable<Task> tasks = _oMCSDbContext.Tasks.Include(a => a.Transaction).Where(@where).AsQueryable();


                if (searchCriteria.Filters != null)
                {

                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        if (filter.Value == "-1")
                        {
                            continue;
                        }

                        if (filter.ColumnName == "Number")
                        {
                            tasks = SortTextByNumber(tasks, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "CreationDate")
                        {
                            tasks = SortTextByCreationDate(tasks, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "Description")
                        {
                            tasks = SortTextByDescription(tasks, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                        else if (filter.ColumnName == "TaskDescription")
                        {
                            tasks = SortTextByTaskDescription(tasks, filter.Value, filter.Type, searchCriteria.CultureName);
                        }

                    }
                }

                rowsCount = tasks.Count();

                if (searchCriteria.MultipleOrderBy != null)
                {
                    searchCriteria.MultipleOrderBy = searchCriteria.MultipleOrderBy.OrderBy(a => a.Index).ToList();
                    foreach (var orderBy in searchCriteria.MultipleOrderBy)
                    {
                        if (orderBy.ColumnName == "CreationDate")
                            tasks = OrderTasksByCreationData(tasks, searchCriteria, orderBy.IsAscending);
                        else if (orderBy.ColumnName == "DeliveryDate")
                            tasks = OrderTasksByDeliveryDate(tasks, searchCriteria, orderBy.IsAscending);
                    }
                }
                else
                {
                    tasks = OrderTasksById(tasks, searchCriteria, false);
                }

                tasks = tasks.Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                                         .Take(searchCriteria.PageSize);

                List<Task> Result = tasks.ToList();

                List<TasksAttachments> tasksAttachments = (from taskAtt in _oMCSDbContext.TasksAttachments
                                                           join task in _oMCSDbContext.Tasks on taskAtt.TaskId equals task.Id
                                                           select taskAtt).ToList();


                foreach (var item in Result)
                {
                    var Attachments = tasksAttachments.Where(ta => ta.TaskId == item.Id).ToList();

                    item.TasksAttachments = Attachments;
                }


                return Result;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Task> OrderTasksByCreationData(IQueryable<Task> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Date);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Date);
            }

            return source;
        }
        private IQueryable<Task> OrderTasksByDeliveryDate(IQueryable<Task> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.DeliveryDate);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.DeliveryDate);
            }

            return source;
        }
        private IQueryable<Task> OrderTasksById(IQueryable<Task> source, SearchCriteriaCustom searchCriteria, bool isAscending)
        {
            if (isAscending)
            {
                source = source.SmartOrderBy(p => p.Id);
            }
            else
            {
                source = source.SmartOrderByDescending(p => p.Id);
            }

            return source;
        }


        private IQueryable<Task> SortTextByNumber(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                int Number = Convert.ToInt32(textValue);
                return source.Where(p => p.Transaction.Number == Number);
            }
            return source;
        }

        private IQueryable<Task> SortTextByCreationDate(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                var dateTime = DateTime.ParseExact(textValue, "d/M/yyyy", null);

                return source.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(dateTime));
            }
            return source;
        }

        private IQueryable<Task> SortTextByDescription(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                string txt = textValue;
                return source.Where(p => p.Transaction.Subject.ToString().ToLower().Contains(txt.ToLower()));
            }
            return source;
        }

        private IQueryable<Task> SortTextByTaskDescription(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            if (textValue != null || textValue != "")
            {
                string txt = textValue;
                return source.Where(p => p.TaskDescription.ToString().ToLower().Contains(txt.ToLower()));
            }
            return source;
        }




        public IList<Task> GetTasks(Expression<Func<Task, bool>> @where)
        {
            try
            {
                IList<Task> tasks = (from task in _oMCSDbContext.Tasks.Where(@where)
                                     select new
                                     {
                                         task.Id,
                                         task.StatusId
                                     }).ToList().Select(t => new Task
                                     {
                                         Id = t.Id,
                                         StatusId = t.StatusId
                                     }).ToList();

                return tasks;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Task GetTaskByIdAndorgUnitId(Expression<Func<Task, bool>> @where)
        {
            try
            {
                Task task = _oMCSDbContext.Tasks.Include(a => a.Transaction).Where(@where).FirstOrDefault();
                return task;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public Task GetTaskById(int taskId)
        {
            try
            {
                return this.FindBy(a => a.Id == taskId);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Task> GetTransactionTasks(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<Task> tasks = (from task in _oMCSDbContext.Tasks
                                          where task.TransactionId == transactionId & task.ParentId == null & task.IsDeleted != true
                                          select task);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        switch (filter.ColumnName)
                        {
                            case "ToOrgUnitName":
                                tasks = FilterByToOrgUnit(tasks, filter.Value, filter.Type, cultureName);

                                break;
                            case "ToUserName":
                                tasks = FilterByToUserName(tasks, filter.Value, filter.Type, cultureName);
                                break;
                            default:
                                PropertyInfo propertyinfo = typeof(Task).GetProperty(filter.ColumnName);

                                if (propertyinfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyinfo.PropertyType))
                                {
                                    tasks = this.SortByText(tasks, filter.Value, filter.Type);
                                }
                                else
                                {
                                    tasks = WhereQuery(tasks, filter.ColumnName, filter.Value, filter.Type);
                                }
                                break;
                        }
                    }
                }

                switch (searchCriteria.OrderBy)
                {
                    case "ToOrgUnitName":
                        tasks = OrderByToOrgUnit(tasks, searchCriteria, cultureName);

                        break;
                    case "ToUserName":
                        tasks = OrderByToUserName(tasks, searchCriteria, cultureName);

                        break;
                    case "TaskDescription":
                        tasks = OrderByTaskDescription(tasks, searchCriteria);

                        break;
                    case "DeliveryDateH":
                        tasks = OrderByDeliveryDateH(tasks, searchCriteria);

                        break;
                    case "Id":
                        tasks = OrderById(tasks, searchCriteria);

                        break;
                }

                rowsCount = tasks.Count();

                tasks = tasks
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                    .Take(searchCriteria.PageSize);

                return tasks.ToList()
                    .Select(t => new Task()
                    {
                        Id = t.Id,
                        ToUserId = t.ToUserId,
                        ToUser = new UserProfile() { LocalName = t.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() },
                        ToOrgUnitId = t.ToOrgUnitId,
                        ToOrgUnit = new OrgUnit() { LocalName = t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() },
                        IsExclusive = t.IsExclusive,
                        DeliveryDate = t.DeliveryDate,
                        DeliveryDateH = t.DeliveryDateH,
                        TaskDescription = t.TaskDescription,
                        Status = t.Status,
                        StatusDescription = t.StatusDescription,
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IList<Task> GetTransactionTasksReply(int transactionId, SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                IQueryable<Task> tasks = (from task in _oMCSDbContext.Tasks
                                          where task.TransactionId == transactionId & task.ParentId != null
                                          select task);

                if (searchCriteria.Filters != null)
                {
                    foreach (Filter filter in searchCriteria.Filters)
                    {
                        switch (filter.ColumnName)
                        {
                            case "ToOrgUnitName":
                                tasks = FilterByToOrgUnit(tasks, filter.Value, filter.Type, cultureName);

                                break;
                            case "ToUserName":
                                tasks = FilterByToUserName(tasks, filter.Value, filter.Type, cultureName);
                                break;
                            default:
                                PropertyInfo propertyinfo = typeof(Task).GetProperty(filter.ColumnName);

                                if (propertyinfo != null && typeof(ILocalizeEntity).IsAssignableFrom(propertyinfo.PropertyType))
                                {
                                    tasks = this.SortByText(tasks, filter.Value, filter.Type);
                                }
                                else
                                {
                                    tasks = WhereQuery(tasks, filter.ColumnName, filter.Value, filter.Type);
                                }
                                break;
                        }
                    }
                }

                switch (searchCriteria.OrderBy)
                {
                    case "ToOrgUnitName":
                        tasks = OrderByToOrgUnit(tasks, searchCriteria, cultureName);

                        break;
                    case "ToUserName":
                        tasks = OrderByToUserName(tasks, searchCriteria, cultureName);

                        break;
                    case "TaskDescription":
                        tasks = OrderByTaskDescription(tasks, searchCriteria);

                        break;
                    case "DeliveryDateH":
                        tasks = OrderByDeliveryDateH(tasks, searchCriteria);

                        break;
                    case "Id":
                        tasks = OrderById(tasks, searchCriteria);

                        break;
                }

                rowsCount = tasks.Count();

                tasks = tasks
                    .Skip((searchCriteria.PageIndex - 1) * searchCriteria.PageSize)
                    .Take(searchCriteria.PageSize);

                return tasks.ToList()
                    .Select(t => new Task()
                    {
                        Id = t.Id,
                        FromUserId = t.FromUserId,
                        FromUser = new UserProfile() { LocalName = t.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() },
                        FromOrgUnitId = t.FromOrgUnitId,
                        FromOrgUnit = new OrgUnit() { LocalName = t.FromOrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() },
                        IsExclusive = t.IsExclusive,
                        DeliveryDate = t.DeliveryDate,
                        DeliveryDateH = t.DeliveryDateH,
                        TaskDescription = t.TaskDescription,
                        Status = t.Status,
                        StatusDescription = t.StatusDescription,
                        TasksAttachments = t.TasksAttachments
                    }).ToList();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<Task> CheckEndTasks()
        {
            try
            {
                int Status = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
                List<Task> tasks = _oMCSDbContext.Tasks.Where(t => !t.IsDeleted && t.DeliveryDate < DateTime.Now.AddDays(-1) && t.Status.Id != Status && t.StatusDescription.IsNullOrEmpty()).ToList();
                if (tasks.Count > 0)
                {
                    foreach (var item in tasks)
                    {
                        item.StatusId = TaskStatus.Complete.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
                        item.IsDeleted = true;
                        int expired = TaskStatus.expired.LookupIdentity(LookupCategory.TaskStatus, string.Empty);
                        item.StatusDescription = _oMCSDbContext.Lookups.FirstOrDefault(l => l.Id == expired).Localizations.Where(l => l.Culture.ShortName == "ar").FirstOrDefault().Text;
                        _oMCSDbContext.Entry(item).State = EntityState.Modified;
                    }
                    _oMCSDbContext.SaveChanges();
                }
                return tasks;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        private IQueryable<Task> FilterByToOrgUnit(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<Task> FilterByToUserName(IQueryable<Task> source, string textValue, FilterType filterType, string culureName)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return source.Where(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Contains(textValue));
                case FilterType.EndsWidth:
                    return source.Where(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.EndsWith(textValue));
                case FilterType.StartsWith:
                    return source.Where(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.StartsWith(textValue));
                case FilterType.Equals:
                    return source.Where(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == culureName).FirstOrDefault().Text.Equals(textValue));
            }

            return source;
        }

        private IQueryable<Task> SortByText(IQueryable<Task> source, string textValue, FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Contains:
                    return (from task in source.Where(t => t.TaskDescription.Contains(textValue))
                            select task);
                case FilterType.EndsWidth:
                    return (from task in source.Where(t => t.TaskDescription.EndsWith(textValue))
                            select task);
                case FilterType.StartsWith:
                    return (from task in source.Where(t => t.TaskDescription.StartsWith(textValue))
                            select task);
                case FilterType.Equals:
                    return (from task in source.Where(t => t.TaskDescription.Equals(textValue))
                            select task);
            }

            return source;
        }

        private IQueryable<Task> OrderByToOrgUnit(IQueryable<Task> source, SearchCriteria searchCriteria, string cultureName)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == cultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.ToOrgUnit.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == cultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Task> OrderByToUserName(IQueryable<Task> source, SearchCriteria searchCriteria, string cultureName)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == cultureName).FirstOrDefault().Text);
            }
            else
            {
                source = source.OrderByDescending(t => t.ToUser.LocalizationIdentifier.Localizations.Where(c => c.Culture.ShortName == cultureName).FirstOrDefault().Text);
            }

            return source;
        }

        private IQueryable<Task> OrderByTaskDescription(IQueryable<Task> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.TaskDescription);
            }
            else
            {
                source = source.OrderByDescending(t => t.TaskDescription);
            }

            return source;
        }

        private IQueryable<Task> OrderByDeliveryDateH(IQueryable<Task> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.DeliveryDateH);
            }
            else
            {
                source = source.OrderByDescending(t => t.DeliveryDateH);
            }

            return source;
        }

        private IQueryable<Task> OrderById(IQueryable<Task> source, SearchCriteria searchCriteria)
        {
            if (searchCriteria.Ascending)
            {
                source = source.OrderBy(t => t.Id);
            }
            else
            {
                source = source.OrderByDescending(t => t.Id);
            }

            return source;
        }

        public IList<Task> SendToUserReminderBeforeTaskEnded(int taskProcessingPeriod, int taskReminderCount)
        {
            DateTime datePeriod = DateTime.Now.AddDays(-taskProcessingPeriod);
            List<Task> tasks = _oMCSDbContext.Tasks.Where(t => t.NumberOfNotifications <= taskReminderCount && t.DeliveryDate <= datePeriod).ToList();
            return tasks;
        }

        public void UpdateTaskReminderBeforeEnded(List<int> ids)
        {
            try
            {
                List<Task> tasks = _oMCSDbContext.Tasks.Where(t => !t.IsDeleted && ids.Contains(t.Id)).ToList();
                if (tasks != null && tasks.Count > 0)
                {
                    foreach (var item in tasks)
                    {
                        item.NumberOfNotifications = item.NumberOfNotifications + 1;
                        _oMCSDbContext.Entry(item).State = EntityState.Modified;
                    }
                    _oMCSDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        #endregion
    }
}
