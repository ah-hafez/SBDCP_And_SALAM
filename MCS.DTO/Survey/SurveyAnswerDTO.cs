using System;

namespace MCS.DTO
{
    public class SurveyAnswerDTO 
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; } 
        public DateTime AnswerDate { get; set; }

    }
}
