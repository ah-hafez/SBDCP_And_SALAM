
namespace MCS.UI.Areas.Admin.Models.Groups
{
    public class PermissionsViewModel
    {
        public AddGroupVM AddGroup { get; set; }
        public EditGroupVM EditGroup { get; set; }
        public GroupVM Group { get; set; }

        public PermissionsViewModel()
        {
            AddGroup = new AddGroupVM();
            EditGroup = new EditGroupVM();
            Group = new GroupVM();
        }
    }
}