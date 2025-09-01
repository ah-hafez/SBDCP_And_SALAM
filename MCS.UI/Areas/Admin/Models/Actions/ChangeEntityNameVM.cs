using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class ChangeEntityNameVM
    {
        public int EntityFromId { get; set; }
        public int EntityToId { get; set; }
        public List<LocalizationVM> EntityFromLocalizations { get; set; } = new List<LocalizationVM> { new LocalizationVM { Text = "" }, new LocalizationVM { Text = "" } };
        public List<LocalizationVM> EntityToLocalizations { get; set; } = new List<LocalizationVM> { new LocalizationVM { Text = "" }, new LocalizationVM { Text = "" } };
    }
}