using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TaskRejectMapper
    {
        public static Task Map(TaskActionDTO taskRejectDTO)
        {
            if (taskRejectDTO == null)
            {
                return null;
            }

            TransactionTaskBL transactionTaskBL = new TransactionTaskBL();

            Task oldTask = transactionTaskBL.GetTaskById(taskRejectDTO.TaskId);

            Task task = new Task()
            {
                StatusDescription = taskRejectDTO.Description,
                Date = oldTask.Date,
                DateH = oldTask.DateH,
                DeliveryDateH = oldTask.DeliveryDateH,
                DeliveryDate = oldTask.DeliveryDate,
                FromOrgUnit = oldTask.FromOrgUnit,
                FromUser = oldTask.FromUser,
                FromUserId = oldTask.FromUserId,
                Id = oldTask.Id,
                IsExclusive = oldTask.IsExclusive,
                LevelLimitation = oldTask.LevelLimitation,
                Parent = oldTask.Parent,
                Reminders = oldTask.Reminders,
                Status = oldTask.Status,
                TaskDescription = oldTask.TaskDescription,
                //TaskWorkflows = oldTask.TaskWorkflows,
                ToOrgUnit = oldTask.ToOrgUnit,
                ToUser = oldTask.ToUser,
                Transaction = oldTask.Transaction
            };

            task.TasksAttachments = new List<TasksAttachments>();
            List<DocumentInfo> documentInfos = taskRejectDTO.Document.Select(d => DocumentMapper.Map(d)).ToList();
            task.TasksAttachments = documentInfos.Select(d =>
            {
                TasksAttachments taskAttachment = new TasksAttachments
                {
                    DocumentInfo = d,
                    TaskId = task.Id
                };
                return taskAttachment;

            }).ToList();
            return task;
        }
    }
}