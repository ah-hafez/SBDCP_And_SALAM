using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Survey
{
    public class SurveyAnswerVM
    {
        public int Id { get; set; } 
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; } 
        public DateTime AnswerDate { get; set; }
        public List<AnswerOptionsVM> answerOptions { get; set; } = new List<AnswerOptionsVM>();

    }
}
