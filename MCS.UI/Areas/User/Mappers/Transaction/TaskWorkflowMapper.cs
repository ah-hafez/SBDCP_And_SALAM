using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TaskWorkflowMapper
    {
        public static List<TaskWorkflowVM> Map(IList<TaskWorkflowDTO> taskWorkflowDTOs)
        {
            if (taskWorkflowDTOs == null || !taskWorkflowDTOs.Any())
            {
                return new List<TaskWorkflowVM>();
            }
            List<TaskWorkflowVM> taskWorkflowVMs = taskWorkflowDTOs
                .Select(taskWorkflowDTO => new TaskWorkflowVM()
                { 
                    FromOrgUnitId = taskWorkflowDTO.FromOrgUnitId,
                    FromUserId = taskWorkflowDTO.FromUserId,
                    ToOrgUnitId = taskWorkflowDTO.ToOrgUnitId,
                    ToUserId = taskWorkflowDTO.ToUserId
                }).ToList();

            return taskWorkflowVMs;
        }
        public static List<TaskWorkflowDTO> Map(IList<TaskWorkflowVM> taskWorkflowVMs)
        {
            if (taskWorkflowVMs == null || !taskWorkflowVMs.Any())
            {
                return new List<TaskWorkflowDTO>();
            }
            List<TaskWorkflowDTO> taskWorkflowDTOs = taskWorkflowVMs
                .Select(taskWorkflowVM => new TaskWorkflowDTO()
                { 
                    FromOrgUnitId = taskWorkflowVM.FromOrgUnitId,
                    FromUserId = taskWorkflowVM.FromUserId,
                    ToOrgUnitId = taskWorkflowVM.ToOrgUnitId,
                    ToUserId = taskWorkflowVM.ToUserId
                }).ToList();

            return taskWorkflowDTOs;
        }


    }
}