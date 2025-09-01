using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TaskHistory : EntityBase
    {
        public virtual Task Parent { get; set; }
        public virtual UserProfile ToUser { get; set; }
        public virtual OrgUnit ToOrgUnit { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public virtual OrgUnit FromOrgUnit { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public virtual Lookup Status { get; set; }
        public string StatusDescription { get; set; }
        public string TaskDescription { get; set; }
        public bool IsExclusive { get; set; }
        public virtual Transaction Transaction { get; set; }
    }
}
