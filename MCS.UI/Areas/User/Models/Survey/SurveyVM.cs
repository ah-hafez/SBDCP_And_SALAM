using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Survey
{
    public class SurveyVM
    {
        public List<SurveyQuestionVM> SurveyQuestion { get; set; } = new List<SurveyQuestionVM>(); 
        public SurveyNoteVM surveyNote { get; set; } = new SurveyNoteVM(); 


    }
}