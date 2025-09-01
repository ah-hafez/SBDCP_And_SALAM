namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class AttachmentTypeViewModel
    {
        public AttachmentTypeVM AttachmentType { get; set; }
        public AttachmentTypeAddVM AddAttachmentType { get; set; }
        public AttachmentTypeEditVM EditAttachmentType { get; set; }

        public AttachmentTypeViewModel()
        {
            AttachmentType = new AttachmentTypeVM();
            AddAttachmentType = new AttachmentTypeAddVM();
            EditAttachmentType = new AttachmentTypeEditVM();
        }
    }
}