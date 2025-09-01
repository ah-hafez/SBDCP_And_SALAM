namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TaskWorkflowVM
    {
        public int FromOrgUnitId { get; set; }
        public int ToOrgUnitId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
    }
}