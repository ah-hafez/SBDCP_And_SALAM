namespace MCS.Domain
{
    public class FollowUpProccess : LookupBase
    {
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
