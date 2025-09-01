using System;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class SubTaskAddVM
    {
        public int FromOrgUnitId { get; set; }

        [CustomDisplayName("User.Task.SubTaskAdd.ToUser")]
        [CustomRequired("User.Task.SubTaskAdd.ToUserRequired")]
        public int ToUserId { get; set; }

        public string ToUserName { get; set; }
        public string ToOrgUnitName { get; set; }

        [CustomDisplayName("User.Task.SubTaskAdd.ToOrgUnit")]
        [CustomRequired("User.Task.SubTaskAdd.ToOrgUnitRequired")]
        public int ToOrgUnitId { get; set; }

        [CustomDisplayName("User.Task.SubTaskAdd.DeliveryDate")]
        [CustomRequired("User.Task.SubTaskAdd.DeliveryDateHRequired")]
        public DateTime DeliveryDate { get; set; }


        [CustomDisplayName("User.Task.SubTaskAdd.DeliveryDateH")]
        // [CustomRequired("User.Task.SubTaskAdd.DeliveryDateHRequired")]
        public string DeliveryDateH { get; set; }


        [CustomDisplayName("User.Task.SubTaskAdd.TaskDescription")]
        [CustomRequired("User.Task.SubTaskAdd.TaskDescriptionRequired")]
        public string TaskDescription { get; set; }

        public DocumentVM Attachment { get; set; }

        public int TransactionId { get; set; }
    }
}