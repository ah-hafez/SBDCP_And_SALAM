using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TaskActionVM
    {
        public int TaskId { get; set; }
        public string Description { get; set; }

        [CustomRequired("User.Task.AcceptTask.SubjectRequired")]
        public string Subject { get; set; }
        public List<DocumentVM> Document { get; set; }
    }
}