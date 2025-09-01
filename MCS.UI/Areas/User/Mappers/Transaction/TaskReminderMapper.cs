using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TaskReminderMapper
    {
        public static List<TaskReminderVM> Map(IList<TaskReminderDTO> taskReminderDTOs)
        {
            if (taskReminderDTOs == null || !taskReminderDTOs.Any())
            {
                return new List<TaskReminderVM>();
            }
            List<TaskReminderVM> taskReminderVMs = taskReminderDTOs
                .Select(taskReminderDTO => new TaskReminderVM()
                { 
                    Date = taskReminderDTO.Date,
                    DateH = taskReminderDTO.DateH,
                    Id = taskReminderDTO.Id
                }).ToList();

            return taskReminderVMs;
        }
        public static List<TaskReminderDTO> Map(IList<TaskReminderVM> taskReminderVMs)
        {
            if (taskReminderVMs == null || !taskReminderVMs.Any())
            {
                return new List<TaskReminderDTO>();
            }
            List<TaskReminderDTO> taskReminderDTOs = taskReminderVMs
                .Select(taskReminderVM => new TaskReminderDTO()
                { 
                    Date = taskReminderVM.Date,
                    DateH = taskReminderVM.DateH,
                    Id = taskReminderVM.Id
                }).ToList();

            return taskReminderDTOs;
        }


    }
}