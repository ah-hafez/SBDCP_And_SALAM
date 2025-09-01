
namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class UserCategoriesViewModel
    {
        public AddUserCategoryVM UserCategoryAddDTO { get; set; }
        public EditUserCategoryVM UserCategoryEditDTO { get; set; }
        public EditUserCategoryTrayVM UserCategoryDTO { get; set; }

        public UserCategoriesViewModel()
        {
            UserCategoryAddDTO = new AddUserCategoryVM();
            UserCategoryEditDTO = new EditUserCategoryVM();
            UserCategoryDTO = new EditUserCategoryTrayVM();
        }
    }
}