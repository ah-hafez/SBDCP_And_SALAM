using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class ActionVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public int TypeId { get; set; }
        public string LocalName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public bool IsAsCopy { get; set; }

    }
}