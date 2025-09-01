using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class AssignmentPaperGroupVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [CustomRequired("الحقل مطلوب")]
        public string Name { get; set; }
        //public List<LocalizationVM> Names { get; set; }
        public int OrderNo { get; set; }
        public int Key { get; set; }
        public int DefaultActionId { get; set; }
        public string DefaultActionName { get; set; }


    }
}