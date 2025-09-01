using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.User.Models
{
    public class ReporterVM
    {
        public int Id { get; set; }
        public int ToEntityId { get; set; }
        public string LocalName { get; set; }
        public List<LocalizationVM> Names { get; set; }
    }
}