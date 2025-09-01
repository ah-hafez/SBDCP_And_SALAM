using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class SurveyAnswer : EntityBase, IAuditable
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; } 
        public DateTime AnswerDate { get; set; }

    }
}
