using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Tray
{
    public class EditTrayVM
    {
        public int Id { get; set; }

        public List<LookupLocalizationVM> Names { get; set; }

        public int PermissionId { get; set; }
    }
}