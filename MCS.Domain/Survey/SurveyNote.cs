using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
   public class SurveyNote : EntityBase, IAuditable
    {
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
    }
}
