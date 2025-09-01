namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class PriorityViewModel
    {
        public PriorityVM Priority { get; set; }
        public PriorityAddVM AddPriority { get; set; }
        public PriorityEditVM EditPriority { get; set; }

        public PriorityViewModel()
        {
            Priority = new PriorityVM();
            AddPriority = new PriorityAddVM();
            EditPriority = new PriorityEditVM();
        }
    }
}