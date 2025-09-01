namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ManagersManagementViewModel
    {
        public ManagerAddVM AddManager { get; set; }
        public ManagerEditVM EditManager { get; set; }
        public ManagerVM Manager { get; set; }

        public ManagersManagementViewModel()
        {
            AddManager = new ManagerAddVM();
            EditManager = new ManagerEditVM();
            Manager = new ManagerVM();
        }
    }
}