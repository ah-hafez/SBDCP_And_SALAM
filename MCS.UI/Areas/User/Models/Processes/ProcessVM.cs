using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Actions
{
    public class ActionVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public int TypeId { get; set; }
        public string LocalName { get; set; }
        public int? SortNo { get; set; }
    }
}