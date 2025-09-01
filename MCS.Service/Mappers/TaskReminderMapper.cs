using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TaskReminderMapper
    {
        public static List<TaskReminderDTO> Map(IList<TaskReminder> taskReminders)
        {
            if (taskReminders == null || !taskReminders.Any())
            {
                return null;
            }

            List<TaskReminderDTO> taskReminderDTOs = taskReminders
                .Select(taskReminder => new TaskReminderDTO()
                {
                    Id = taskReminder.Id,
                    Date = taskReminder.Date,
                    DateH = taskReminder.DateH
                }).ToList();

            return taskReminderDTOs;
        }

        public static List<TaskReminder> Map(IList<TaskReminderDTO> taskReminderDTOs)
        {
            if (taskReminderDTOs == null || !taskReminderDTOs.Any())
            {
                return null;
            }
            List<TaskReminder> taskReminders = taskReminderDTOs
                .Select(taskReminderDTO => new TaskReminder()
                {
                    Id = taskReminderDTO.Id,
                    Date = taskReminderDTO.Date,
                    DateH = taskReminderDTO.DateH
                }).ToList();

            return taskReminders;
        }
    }
}