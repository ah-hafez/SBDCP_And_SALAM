using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TaskWorkflowMapper
    {
        public static TaskWorkflow Map(TaskWorkflowDTO taskWorkflowDTO)
        {
            if (taskWorkflowDTO == null)
            {
                return null;
            }
            IOrgUnitBL organizationUnitBL = IoC.Resolve<IOrgUnitBL>();

            TaskWorkflow taskWorkflow = new TaskWorkflow()
            {
                FromEntity = organizationUnitBL.GetOrgUnitById(taskWorkflowDTO.FromOrgUnitId),
                ToEntity = organizationUnitBL.GetOrgUnitById(taskWorkflowDTO.ToOrgUnitId),
            };

            return taskWorkflow;
        }

        public static List<TaskWorkflow> Map(IList<TaskWorkflowDTO> taskWorkflowDTOs)
        {
            if (taskWorkflowDTOs == null || !taskWorkflowDTOs.Any())
            {
                return null;
            }

            List<TaskWorkflow> taskWorkflows = new List<TaskWorkflow>();

            foreach (TaskWorkflowDTO taskWorkflowDTO in taskWorkflowDTOs)
            {
                TaskWorkflow taskWorkflow = TaskWorkflowMapper.Map(taskWorkflowDTO);

                taskWorkflows.Add(taskWorkflow);
            }

            return taskWorkflows;
        }
        public static List<TaskWorkflowDTO> Map(IList<TaskWorkflow> taskWorkflowDTOs)
        {
            if (taskWorkflowDTOs == null || !taskWorkflowDTOs.Any())
            {
                return null;
            }
            IOrgUnitBL organizationUnitBL = IoC.Resolve<IOrgUnitBL>();
            List<TaskWorkflowDTO> taskWorkflows = taskWorkflowDTOs
                .Select(taskWorkflowDTO => new TaskWorkflowDTO
                {
                    FromOrgUnitId = taskWorkflowDTO.FromEntity.Id,
                    ToOrgUnitId = taskWorkflowDTO.ToEntity.Id,

                }).ToList();

            return taskWorkflows;
        }
    }
}