using System;
using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.ReleaseNotes
{
    public class ReleaseNotesVM
    {
        public int Id { get; set; }
        public string ReleaseNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DateHj { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}