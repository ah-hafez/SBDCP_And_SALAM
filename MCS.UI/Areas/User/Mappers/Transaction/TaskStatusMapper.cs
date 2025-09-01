using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TaskStatusMapper
    {
        public static List<TaskStatusVM> Map(IList<TaskStatusDTO> taskStatusDTOs)
        {
            if (taskStatusDTOs == null || !taskStatusDTOs.Any())
            {
                return new List<TaskStatusVM>();
            }
            List<TaskStatusVM> taskStatusVMs = taskStatusDTOs
                .Select(taskStatusDTO => new TaskStatusVM()
                {
                    Id = taskStatusDTO.Id,
                    StatusId = taskStatusDTO.StatusId
                }).ToList();

            return taskStatusVMs;
        }
        public static List<TaskStatusDTO> Map(IList<TaskStatusVM> taskStatusVMs)
        {
            if (taskStatusVMs == null || !taskStatusVMs.Any())
            {
                return new List<TaskStatusDTO>();
            }
            List<TaskStatusDTO> taskStatusDTOs = taskStatusVMs
                .Select(taskStatusVM => new TaskStatusDTO()
                { 
                    Id = taskStatusVM.Id,
                    StatusId = taskStatusVM.StatusId
                }).ToList();

            return taskStatusDTOs;
        }


    }
}