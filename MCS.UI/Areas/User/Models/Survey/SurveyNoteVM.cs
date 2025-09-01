using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Survey
{ 
   public class SurveyNoteVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
    }
}
