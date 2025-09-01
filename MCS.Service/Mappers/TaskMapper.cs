using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TaskMapper
    {

        public static ReceivedTaskDTO MapReceivedTask(Task task, string cultureName)
        {
            if (task == null)
                return null;

            ReceivedTaskDTO taskDTO = new ReceivedTaskDTO()
            {
                Id = task.Id,
                TaskDescription = task.TaskDescription,
                DeliveryDate = task.DeliveryDate,
                DeliveryDateH = task.DeliveryDateH,
                FromOrgUnit = task.FromOrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                FromUser = task.FromUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                Subject = task.Transaction.Subject,
                Reminders = TaskReminderMapper.Map(task.Reminders),
                LevelLimitation = task.LevelLimitation,
                IsExclusive = task.IsExclusive,
                Notes = task.StatusDescription
            };
            if (task.TasksAttachments != null)
            {
                taskDTO.Attachment = task.TasksAttachments.Select(d => DocumentMapper.Map(d.DocumentInfo)).ToList();

            }
            if (task.Transaction != null)
            {
                taskDTO.TransactionId = task.Transaction.Id;
                taskDTO.TransactionNumber = task.Transaction.Number;
                taskDTO.TransactionCategoryId = task.Transaction.TransactionCategoryId;
            }

            return taskDTO;
        }

        public static List<ReceivedTaskDTO> MapReceivedTask(IList<Task> tasks, string cultureName)
        {
            if (tasks == null || !tasks.Any())
            {
                return null;
            }

            List<ReceivedTaskDTO> taskDTOs = new List<ReceivedTaskDTO>();

            foreach (Task task in tasks)
            {
                ReceivedTaskDTO taskDTO = TaskMapper.MapReceivedTask(task, cultureName);

                taskDTOs.Add(taskDTO);
            }

            return taskDTOs;
        }

        public static SentTaskDTO MapSentTask(Task task, string cultureName)
        {
            if (task == null)
            {
                return null;
            }
            SentTaskDTO sentTaskDTO = new SentTaskDTO()
            {
                TaskDescription = task.TaskDescription,
                DeliveryDate = task.DeliveryDate,
                DeliveryDateH = task.DeliveryDateH,
                ToOrgUnit = task.ToOrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                ToUser = task.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                Id = task.Id,
                StatusName = task.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText(),
                Status = (TaskStatus)task.Status.Id.LookupInternalID(LookupCategory.TaskStatus, cultureName),
                Notes = task.StatusDescription,


            };

            if (task.Transaction != null)
                sentTaskDTO.TransactionNumber = task.Transaction.Number;
            return sentTaskDTO;
        }

        public static List<SentTaskDTO> MapSentTask(IList<Task> tasks, string cultureName)
        {
            if (tasks == null || !tasks.Any())
            {
                return null;
            }

            List<SentTaskDTO> sentTaskDTOs = new List<SentTaskDTO>();

            foreach (Task task in tasks)
            {
                SentTaskDTO taskDTO = TaskMapper.MapSentTask(task, cultureName);

                sentTaskDTOs.Add(taskDTO);
            }

            return sentTaskDTOs;
        }

        public static List<ReceivedTaskDTO> MapTasksReply(IList<Task> tasks, string cultureName)
        {
            if (tasks == null || !tasks.Any())
            {
                return new List<ReceivedTaskDTO>();
            }

            List<ReceivedTaskDTO> receivedTaskDTOs = tasks.Select(rt =>
            {
                ReceivedTaskDTO receivedTaskDTO = new ReceivedTaskDTO
                {
                    Id = rt.Id,
                    FromUser = rt.FromUser.LocalName,
                    FromOrgUnit = rt.FromOrgUnit.LocalName,
                    Notes = rt.StatusDescription,
                    DeliveryDate = rt.DeliveryDate,
                    DeliveryDateH = rt.DeliveryDateH,
                };
                return receivedTaskDTO;
            }).ToList();
            return receivedTaskDTOs;
        }
        public static List<TaskAddDTO> MapTasks(IList<Task> tasks, string cultureName)
        {
            if (tasks == null || !tasks.Any())
            {
                return new List<TaskAddDTO>();
            }
            List<TaskAddDTO> taskDTOs = tasks.Select(task => new TaskAddDTO()
            {
                Id = task.Id,
                ToUserId = task.ToUserId,
                ToUserName = task.ToUser.LocalName,
                ToOrgUnitId = task.ToOrgUnitId,
                ToOrgUnitName = task.ToOrgUnit.LocalName,
                IsExclusive = task.IsExclusive,
                DeliveryDate = task.DeliveryDate,
                DeliveryDateH = task.DeliveryDateH,
                TaskDescription = task.TaskDescription,
                Status = task.Status.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                StatusId = task.Status.Id,
                Notes = task.StatusDescription,
            }).ToList();


            return taskDTOs;
        }

        public static List<TaskStatusDTO> MapTasksStatus(IList<Task> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                return null;
            }
            List<TaskStatusDTO> taskStatusDTOs = new List<TaskStatusDTO>();

            foreach (Task task in tasks)
            {
                TaskStatusDTO taskStatusDTO = new TaskStatusDTO() { Id = task.Id, StatusId = task.StatusId };

                taskStatusDTOs.Add(taskStatusDTO);
            }

            return taskStatusDTOs;
        }

        public static List<TaskAttachmentsDTO> Map(IList<TasksAttachments> tasksAttachments)
        {
            if (tasksAttachments == null)
            {
                return new List<TaskAttachmentsDTO>();
            }
            List<TaskAttachmentsDTO> taskAttachmentsDTOs = tasksAttachments.Select(ta =>
            {
                TaskAttachmentsDTO taskAttachmentsDTO = new TaskAttachmentsDTO
                {
                    Id = ta.Id,
                    DocumentId = ta.DocumentInfoId,
                    TaskId = ta.TaskId,
                    Attachment = DocumentMapper.Map(ta.DocumentInfo)
                };
                return taskAttachmentsDTO;
            }).ToList();

            return taskAttachmentsDTOs;
        }

        public static TaskAttachmentsDTO Map(TasksAttachments tasksAttachment)
        {
            if (tasksAttachment == null)
            {
                return new TaskAttachmentsDTO();
            }

            TaskAttachmentsDTO taskAttachmentsDTO = new TaskAttachmentsDTO
            {
                Id = tasksAttachment.Id,
                DocumentId = tasksAttachment.DocumentInfoId,
                TaskId = tasksAttachment.TaskId,
                Attachment = DocumentMapper.Map(tasksAttachment.DocumentInfo)
            };
            return taskAttachmentsDTO;
        }

        public static TaskLightDTO Map(Task task)
        {
            if (task == null)
            {
                return null;
            }
            TaskLightDTO taskLight = new TaskLightDTO()
            {
                Id = task.Id,
                StatusId = task.StatusId,
                TransactionCategoryId = task.Transaction.TransactionCategoryId
            };
            return taskLight;
        }
    }
}