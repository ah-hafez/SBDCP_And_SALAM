using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class DistributionListDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int LocalizationIdentifierId { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public List<LocalizationDTO> Name { get; set; }
        public IList<DistributionListDetailsDTO> DistributionListDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
