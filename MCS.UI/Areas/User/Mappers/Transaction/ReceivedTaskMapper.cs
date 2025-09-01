using System;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class ReceivedTaskMapper
    {
        public static List<ReceivedTaskVM> Map(IList<ReceivedTaskDTO> ReceivedTaskDTOs)
        {

            if (ReceivedTaskDTOs == null || !ReceivedTaskDTOs.Any())
            {
                return new List<ReceivedTaskVM>();
            }
            List<ReceivedTaskVM> ReceivedTaskVMs = ReceivedTaskDTOs
                .Select(ReceivedTaskDTO => new ReceivedTaskVM()
                {
                    Attachment = DocumentMapper.Map(ReceivedTaskDTO.Attachment),
                    DeliveryDate = ReceivedTaskDTO.DeliveryDate,
                    DeliveryDateH = ReceivedTaskDTO.DeliveryDateH,
                    FromOrgUnit = ReceivedTaskDTO.FromOrgUnit,
                    Id = ReceivedTaskDTO.Id,
                    IsExclusive = ReceivedTaskDTO.IsExclusive,
                    LevelLimitation = ReceivedTaskDTO.LevelLimitation,
                    Reminders = TaskReminderMapper.Map(ReceivedTaskDTO.Reminders),
                    Subject = ReceivedTaskDTO.Subject,
                    TaskDescription = ReceivedTaskDTO.TaskDescription,
                    TransactionId = ReceivedTaskDTO.TransactionId,
                    TransactionNumber = ReceivedTaskDTO.TransactionNumber,
                    FromUser = ReceivedTaskDTO.FromUser,
                    TransactionCategoryId = ReceivedTaskDTO.TransactionCategoryId,
                    Notes = ReceivedTaskDTO.Notes,

                    DelayedDaysCount = (Int32.Parse((DateTime.Now.Date - ReceivedTaskDTO.DeliveryDate.Date).Days.ToString()).ToString()),


                }).ToList();

            return ReceivedTaskVMs;
        }
        public static List<ReceivedTaskDTO> Map(IList<ReceivedTaskVM> ReceivedTaskVMs)
        {

            if (ReceivedTaskVMs == null || !ReceivedTaskVMs.Any())
            {
                return new List<ReceivedTaskDTO>();
            }
            List<ReceivedTaskDTO> ReceivedTaskDTOs = ReceivedTaskVMs
                .Select(ReceivedTaskVM => new ReceivedTaskDTO()
                {
                    Attachment = DocumentMapper.Map(ReceivedTaskVM.Attachment),
                    DeliveryDate = ReceivedTaskVM.DeliveryDate,
                    DeliveryDateH = ReceivedTaskVM.DeliveryDateH,
                    FromOrgUnit = ReceivedTaskVM.FromOrgUnit,
                    Id = ReceivedTaskVM.Id,
                    IsExclusive = ReceivedTaskVM.IsExclusive,
                    LevelLimitation = ReceivedTaskVM.LevelLimitation,
                    Reminders = TaskReminderMapper.Map(ReceivedTaskVM.Reminders),
                    Subject = ReceivedTaskVM.Subject,
                    TaskDescription = ReceivedTaskVM.TaskDescription,
                    TransactionId = ReceivedTaskVM.TransactionId,
                    TransactionNumber = ReceivedTaskVM.TransactionNumber,
                    Notes = ReceivedTaskVM.Notes,

                }).ToList();

            return ReceivedTaskDTOs;
        }
        public static ReceivedTaskVM Map(ReceivedTaskDTO ReceivedTaskDTO)
        {
            if (ReceivedTaskDTO != null)
            {
                return new ReceivedTaskVM()
                {
                    Attachment = DocumentMapper.Map(ReceivedTaskDTO.Attachment),
                    DeliveryDate = ReceivedTaskDTO.DeliveryDate,
                    DeliveryDateH = ReceivedTaskDTO.DeliveryDateH,
                    FromOrgUnit = ReceivedTaskDTO.FromOrgUnit,
                    Id = ReceivedTaskDTO.Id,
                    IsExclusive = ReceivedTaskDTO.IsExclusive,
                    LevelLimitation = ReceivedTaskDTO.LevelLimitation,
                    Reminders = TaskReminderMapper.Map(ReceivedTaskDTO.Reminders),
                    Subject = ReceivedTaskDTO.Subject,
                    TaskDescription = ReceivedTaskDTO.TaskDescription,
                    TransactionId = ReceivedTaskDTO.TransactionId,
                    TransactionNumber = ReceivedTaskDTO.TransactionNumber,
                    Notes = ReceivedTaskDTO.Notes,
                };
            }
            return new ReceivedTaskVM();
        }

        public static ReceivedTaskDTO Map(ReceivedTaskVM ReceivedTaskVM)
        {
            if (ReceivedTaskVM != null)
            {
                return new ReceivedTaskDTO()
                {
                    Attachment = DocumentMapper.Map(ReceivedTaskVM.Attachment),
                    DeliveryDate = ReceivedTaskVM.DeliveryDate,
                    DeliveryDateH = ReceivedTaskVM.DeliveryDateH,
                    FromOrgUnit = ReceivedTaskVM.FromOrgUnit,
                    Id = ReceivedTaskVM.Id,
                    IsExclusive = ReceivedTaskVM.IsExclusive,
                    LevelLimitation = ReceivedTaskVM.LevelLimitation,
                    Reminders = TaskReminderMapper.Map(ReceivedTaskVM.Reminders),
                    Subject = ReceivedTaskVM.Subject,
                    TaskDescription = ReceivedTaskVM.TaskDescription,
                    TransactionId = ReceivedTaskVM.TransactionId,
                    TransactionNumber = ReceivedTaskVM.TransactionNumber,
                    Notes= ReceivedTaskVM.Notes,
                };
            }
            return new ReceivedTaskDTO();
        }

        public static List<TaskAttachmentsVM> Map(IList<TaskAttachmentsDTO> taskAttachmentsDTOs)
        {
            if (taskAttachmentsDTOs == null)
            {
                return new List<TaskAttachmentsVM>();
            }
            List<TaskAttachmentsVM> taskAttachmentsVMs = taskAttachmentsDTOs.Select(ta =>
            {
                TaskAttachmentsVM taskAttachmentsVM = new TaskAttachmentsVM
                {
                    Id = ta.Id,
                    DocumentId = ta.DocumentId,
                    TaskId = ta.TaskId,
                    Attachment = DocumentMapper.Map(ta.Attachment)
                };
                return taskAttachmentsVM;
            }).ToList();

            return taskAttachmentsVMs;
        }
    }
}