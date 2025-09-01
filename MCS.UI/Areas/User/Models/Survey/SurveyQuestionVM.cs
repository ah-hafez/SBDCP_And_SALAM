using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Survey
{
    public class SurveyQuestionVM
    {
        public int Id { get; set; }
        public string QuestionsDesc { get; set; }
        public bool IsDeleted { get; set; }
        public int Number { get; set; }
        public SurveyAnswerVM SurveyAnswer { get; set; } = new SurveyAnswerVM();
    }
}
