using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TasksAttachments : EntityBase
    {
        public int TaskId { get; set; }
        public int DocumentInfoId { get; set; }
        public virtual Task Task { get; set; }
        public virtual DocumentInfo DocumentInfo { get; set; }
    }
}
