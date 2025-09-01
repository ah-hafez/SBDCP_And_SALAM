using System;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class SurveyQuestion : EntityBase, IAuditable
    {
        public string QuestionsDesc { get; set; }
        public bool IsDeleted { get; set; }

    }
}
