namespace MCS.UI.Areas.User.Models.Collaboration
{
    public class CollaborationUserInfoVM
    {
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string UserName { get; set; }
        public int NotificationCount { get; set; }
        public int Status { get; set; }
    }
}