using System;

namespace MCS.DTO
{ 
   public class SurveyNoteDTO  
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
    }
}
