namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SpecificLevelViewModel
    {
        public SpecificLevelVM SpecificLevel { get; set; }
        public SpecificLevelAddVM AddSpecificLevel { get; set; }
        public SpecificLevelEditVM EditSpecificLevel { get; set; }

        public SpecificLevelViewModel()
        {
            SpecificLevel = new SpecificLevelVM();
            AddSpecificLevel = new SpecificLevelAddVM();
            EditSpecificLevel = new SpecificLevelEditVM();
        }
    }
}