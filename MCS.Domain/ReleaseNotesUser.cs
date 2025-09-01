using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class ReleaseNotesUser : EntityBase
    {
        public int ReleaseNoteId { get; set; }
        public int UserId { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual ReleaseNote ReleaseNote { get; set; }
    }
}
