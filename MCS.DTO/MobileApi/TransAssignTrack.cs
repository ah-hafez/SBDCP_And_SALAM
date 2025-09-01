using System.Collections.Generic;

namespace MobileApi.Domain
{
    public class TransAssignTrack
    {
        public string PhysicalEntity { get; set; }

        public string ElcEntity { get; set; }

        public string PhysicalUser { get; set; }

        public string ElcUser { get; set; }

        public string PhysicalDate { get; set; }

        public string ElcDate { get; set; }

        public int ElcUserId { get; set; }

        public int ElcEntityId { get; set; }

        public List<AssignTrackEntity> Assignments { get; set; }

    }

    public class AssignTrackEntity
    {
        public string FromPerson { get; set; }

        public string FromEntity { get; set; }

        public string ToPerson { get; set; }

        public string ToEntity { get; set; }

        public string Date { get; set; }

        public string ProcessName { get; set; }

        public string Remarks { get; set; }
    }
}
