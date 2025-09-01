using System;

namespace MCS.DTO
{
    public class DistributionListDetailsDTO
    {
        public int Id { get; set; }
        public int DistributionListId { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
