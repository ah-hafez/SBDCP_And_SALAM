namespace MCS.DTO
{
    public class PriorityExceptionDTO
    {
        public int Id { get; set; }
        public int PriorityId { get; set; }
        public int OrgUnitId { get; set; }
        public int UserId { get; set; }
        public int LateOnUsersAfter { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
    }
}
