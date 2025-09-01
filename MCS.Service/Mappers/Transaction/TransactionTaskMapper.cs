using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionTaskMapper
    {
        public static List<Task> Map(TransactionTaskDTO transactionTaskDTO)
        {
            if (transactionTaskDTO == null || !transactionTaskDTO.TaskDTOs.Any())
            {
                return null;
            }

            List<Task> tasks = transactionTaskDTO.TaskDTOs
                .Select(taskDTO => new Task
                {
                    Id = taskDTO.Id,
                    ToUserId = taskDTO.ToUserId,
                    ToOrgUnitId = taskDTO.ToOrgUnitId,
                    FromOrgUnitId = taskDTO.FromOrgUnitId,
                    TasksAttachments = Map(taskDTO.Attachment, taskDTO.Id),
                    IsExclusive = taskDTO.IsExclusive,
                    TaskDescription = taskDTO.TaskDescription,
                    DeliveryDate = taskDTO.DeliveryDate,
                    DeliveryDateH = taskDTO.DeliveryDateH,
                    StatusId = taskDTO.StatusId,
                    StatusDescription = taskDTO.Notes
                    
                }).ToList();


            return tasks;
        }
        public static TransactionTaskDTO Map(List<Task> tasks)
        {
            if (tasks == null || !tasks.Any())
            {
                return null;
            }
            var transactionTaskDTO = new TransactionTaskDTO()
            {
                TransactionId = 0,
                TaskDTOs = tasks
                .Select(dto => new TaskAddDTO
                {
                    ToUserId = dto.ToUserId,
                    ToOrgUnitId = dto.ToOrgUnitId,
                    FromOrgUnitId = dto.FromOrgUnitId,
                    Attachment = Map(dto.TasksAttachments),
                    IsExclusive = dto.IsExclusive,
                    TaskDescription = dto.TaskDescription,
                    DeliveryDate = dto.DeliveryDate,
                    DeliveryDateH = dto.DeliveryDateH,
                    Notes = dto.StatusDescription
                }).ToList()
            };

            return transactionTaskDTO;
        }


        public static List<DocumentDTO> Map(List<TasksAttachments> tasksAttachments)
        {
            if (tasksAttachments == null || !tasksAttachments.Any())
            {
                return null;
            }

            List<DocumentDTO> documentDTOs = tasksAttachments
                .Select(taskAttachment => DocumentMapper.Map(taskAttachment.DocumentInfo)).ToList();

            return documentDTOs;
        }

        public static List<TasksAttachments> Map(List<DocumentDTO> documentDTOs, int TaskId)
        {
            if (documentDTOs == null || !documentDTOs.Any())
            {
                return null;
            }

            List<TasksAttachments> taskAttachments = documentDTOs
                .Select(doc => new TasksAttachments
                {
                    TaskId = TaskId,
                    DocumentInfo = DocumentMapper.Map(doc)
                }).ToList();


            return taskAttachments;
        }
        //public static List<Task> Map(TransactionSubTaskDTO transactionSubTaskDTO)
        //{
        //    if (transactionSubTaskDTO == null || !transactionSubTaskDTO.SubTasks.Any())
        //    {
        //        return null;
        //    }
        //    IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
        //    TransactionTaskBL transactionTaskBL = new TransactionTaskBL();
        //    IOrgUnitBL organizationUnitBL = IoC.Resolve<IOrgUnitBL>();
        //    List<Task> tasks = new List<Task>();

        //    Task parentTask = transactionTaskBL.GetTaskById(transactionSubTaskDTO.ParentId);

        //    foreach (SubTaskAddDTO subTaskAddDTO in transactionSubTaskDTO.SubTasks)
        //    {
        //        Task task = new Task()
        //        {
        //            ToUser = userManagementBL.GetUserById(subTaskAddDTO.ToUserId),
        //            ToOrgUnit = organizationUnitBL.GetOrgUnitById(subTaskAddDTO.ToOrgUnitId),
        //            FromOrgUnit = organizationUnitBL.GetOrgUnitById(subTaskAddDTO.FromOrgUnitId),
        //            DocumentInfo = DocumentMapper.Map(subTaskAddDTO.Attachment),
        //            TaskDescription = subTaskAddDTO.TaskDescription,
        //            DeliveryDate = subTaskAddDTO.DeliveryDate,
        //            Parent = parentTask
        //        };

        //        tasks.Add(task);
        //    }

        //    return tasks;
        //}
    }
}