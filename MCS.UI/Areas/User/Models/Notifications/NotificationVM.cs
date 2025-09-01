namespace MCS.UI.Areas.User.Models.Notifications
{
    public class NotificationVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string Date { get; set; }
        public string Sender { get; set; }
        public bool IsRead { get; set; }
        public int NotificationTemplateTypeId { get; set; }
    }
}