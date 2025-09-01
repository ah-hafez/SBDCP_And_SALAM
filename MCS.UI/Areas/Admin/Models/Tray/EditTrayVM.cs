using System.Collections.Generic;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Tray
{
    public class EditTrayVM
    {
        public int Id { get; set; }

        public List<LookupLocalizationVM> Names { get; set; }

        public int PermissionId { get; set; }
    }
}