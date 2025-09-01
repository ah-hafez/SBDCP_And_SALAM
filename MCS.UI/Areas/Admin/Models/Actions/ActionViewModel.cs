namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class ActionViewModel
    {
        public AddActionVM AddAction { get; set; }
        public EditActionVM EditAction { get; set; }
        public ActionVM Action { get; set; }

        public ActionViewModel()
        {
            AddAction = new AddActionVM();
            EditAction = new EditActionVM();
            Action = new ActionVM();
        }
    }
}