using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Tray
{
    public class TrayVM
    {
        public int Id { get; set; }

        public IList<LookupLocalizationVM> Names { get; set; }

        public string LocalName { get; set; }

        //public PermissionDTO Permission { get; set; }
        public string Permission { get; set; }

        public bool IsSelected { get; set; }

        public int sort { get; set; }
    }
}