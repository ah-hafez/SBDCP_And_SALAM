using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class AddActionVM
    {
        public List<LocalizationVM> Description { get; set; }

        public int TypeId { get; set; }

        public bool IsAsCopy { get; set; }
    }
}