using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.UserManagement;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TaskAddVM
    {
        public TaskAddVM()
        {

            TaskWorkflows = new List<TaskWorkflowVM>();
            OrgStructure = new List<OrgStructureInfoVM>();
        }
        public int Id { get; set; }

        public int Key { get; set; }

        public int ReceivedFromOrgUnitId { get; set; }
        public OrgUnitVM ReceivedFromOrgUnit { get; set; }
        public UserProfileVM ReceivedFromUser { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.ToUser")]
        //[CustomRequired("User.Task.TaskAdd.ToUserRequired")]
        public int? SentToUserId { get; set; }

        public string SentToUserName { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.ToOrgUnit")]
        [CustomRequired("User.Task.TaskAdd.ToOrgUnitRequired")]
        public int SentToOrgUnitId { get; set; }

        public string SentToOrgUnitName { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.EndDate")]
        [CustomRequired("User.Task.TaskAdd.DeliveryDateHRequired")]
        public string DeliveryDate { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.DeliveryDateH")]
        public string DeliveryDateH { get; set; }

        public bool IsExclusive { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.TaskDescription")]
        [CustomRequired("User.Task.TaskAdd.TaskDescriptionRequired")]
        [CustomStringLength("User.Task.TaskAdd.TaskDescriptionLength", 255, 6)]
        public string TaskDescription { get; set; }

        public List<DocumentVM> Attachment { get; set; }

        public List<TaskWorkflowVM> TaskWorkflows { get; set; }

        public string OrgSettings { get; set; }

        public List<OrgStructureInfoVM> OrgStructure { get; set; }

        [CustomDisplayName("User.Task.TaskAdd.ActionId")]
        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        //[CustomRequired("User.Task.TaskAdd.ActionRequired")]
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public object[] ActionTypeId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string Notes { get; set; }
        public TransactionDetailsInfoVM TransactionDetailsInfoVM { get; set; }
        public AjaxGrid<TaskAddVM> TasksGrid { get; set; } = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, false);
        public AjaxGrid<ReceivedTaskVM> TasksReplyGrid { get; set; } = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(new List<ReceivedTaskVM>(), 1, 0, false);
        public string DelayedDaysCount { get; set; }
    }
}