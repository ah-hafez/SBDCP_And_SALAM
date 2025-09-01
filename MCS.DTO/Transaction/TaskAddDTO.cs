using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TaskAddDTO
    {
        public TaskAddDTO()
        {

            TaskWorkflows = new List<TaskWorkflowDTO>();
            OrgStructure = new List<OrgStructureInfoDTO>();
        }
        public int Id { get; set; }
        public int FromOrgUnitId { get; set; }

        //[CustomDisplayName("User.Task.TaskAdd.ToUser")]
        [CustomRequired("User.Task.TaskAdd.ToUserRequired")]
        public int ToUserId { get; set; }

        public string ToUserName { get; set; }

        //[CustomDisplayName("User.Task.TaskAdd.ToOrgUnit")]
        [CustomRequired("User.Task.TaskAdd.ToOrgUnitRequired")]
        public int ToOrgUnitId { get; set; }

        public string ToOrgUnitName { get; set; }
        
        public DateTime DeliveryDate { get; set; }
        
        public string DeliveryDateH { get; set; }

        public bool IsExclusive { get; set; }
        public string TaskDescription { get; set; }

        public List<DocumentDTO> Attachment { get; set; }

        public List<TaskWorkflowDTO> TaskWorkflows { get; set; }

        public string OrgSettings { get; set; }

        public List<OrgStructureInfoDTO> OrgStructure { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public object[] ActionTypeId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string Notes { get; set; }
        public int TransactionId { get; set; }
        public string DelayedDaysCount { get; set; }



    }
}
