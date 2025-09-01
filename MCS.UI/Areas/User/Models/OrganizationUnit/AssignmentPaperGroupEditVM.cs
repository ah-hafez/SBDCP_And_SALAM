using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class AssignmentPaperGroupEditVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        //public List<LocalizationVM> Names { get; set; }
        public int OrderNo { get; set; }
        public int Key { get; set; }
        public int DefaultActionId { get; set; }
        public int DefaultActionName { get; set; }

    }
}