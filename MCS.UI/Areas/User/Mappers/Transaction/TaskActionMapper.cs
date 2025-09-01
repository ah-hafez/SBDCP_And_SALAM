using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TaskActionMapper
    {
        public static List<TaskActionVM> Map(IList<TaskActionDTO> taskActionDTOs)
        {
            if (taskActionDTOs == null || !taskActionDTOs.Any())
            {
                return new List<TaskActionVM>();
            }
            List<TaskActionVM> taskActionVMs = taskActionDTOs
                .Select(taskActionDTO => new TaskActionVM()
                { 
                    Description = taskActionDTO.Description,
                    Document = DocumentMapper.Map(taskActionDTO.Document),
                    Subject = taskActionDTO.Subject,
                    TaskId = taskActionDTO.TaskId
                }).ToList();

            return taskActionVMs;
        }
        public static List<TaskActionDTO> Map(IList<TaskActionVM> taskActionVMs)
        {
            if (taskActionVMs == null || !taskActionVMs.Any())
            {
                return new List<TaskActionDTO>();
            }
            List<TaskActionDTO> taskActionDTOs = taskActionVMs
                .Select(taskActionVM => new TaskActionDTO()
                { 
                    Description = taskActionVM.Description,
                    Document = DocumentMapper.Map(taskActionVM.Document),
                    Subject = taskActionVM.Subject,
                    TaskId = taskActionVM.TaskId
                }).ToList();

            return taskActionDTOs;
        }
        public static TaskActionDTO Map(TaskActionVM taskActionVM)
        {
            if (taskActionVM == null)
            {
                return null;
            }
            TaskActionDTO taskActionDTO = new TaskActionDTO()
                {
                    Description = taskActionVM.Description,
                    Document = DocumentMapper.Map(taskActionVM.Document),
                    Subject = taskActionVM.Subject,
                    TaskId = taskActionVM.TaskId
                };

            return taskActionDTO;
        }

        public static List<TaskAttachmentsDTO> Map(List<TaskAttachmentsVM> taskAttachmentsVMs)
        {
            if (taskAttachmentsVMs == null || !taskAttachmentsVMs.Any())
            {
                return null;
            }

            List<TaskAttachmentsDTO> taskAttachmentsDTOs = taskAttachmentsVMs
                .Select(taskAttachment => new TaskAttachmentsDTO
                {
                    Id = taskAttachment.Id,
                    TaskId = taskAttachment.TaskId,
                    DocumentId = taskAttachment.DocumentId,
                    Attachment = DocumentMapper.Map(taskAttachment.Attachment)
                }).ToList();


            return taskAttachmentsDTOs;
        }

        public static List<TaskAttachmentsVM> Map(List<TaskAttachmentsDTO> taskAttachmentsDTOs)
        {
            if (taskAttachmentsDTOs == null || !taskAttachmentsDTOs.Any())
            {
                return null;
            }

            List<TaskAttachmentsVM> taskAttachmentsVMs = taskAttachmentsDTOs
                .Select(TaskAttachment => new TaskAttachmentsVM
                {
                    Id = TaskAttachment.Id,
                    TaskId = TaskAttachment.TaskId,
                    DocumentId = TaskAttachment.DocumentId,
                    Attachment = DocumentMapper.Map(TaskAttachment.Attachment)
                }).ToList();


            return taskAttachmentsVMs;
        }
    }
}