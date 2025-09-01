using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IUserCategoryRepository : IRepository<UserCategory>
    {

        int AddUserCategory(UserCategory userCategory);
        void UpdateUserCategory(UserCategory userCategory);
        void UpdateUsersCategoriesTrays(IList<UserCategoryTray> usersCategoriesTrays);
        void DeleteUserCategory(int id);
        UserCategory GetUserCategoryById(int userCategoryId);
        IList<Tray> GetUserCategoryTrays(int userCategoryId);
        IList<Tray> GetUserCategoryTrays(int userCategoryId, string cultureName);
        IList<UserCategory> GetAllUsersCategoriesTrays(string cultureName);
        IList<UserCategory> GetUserCategories(SearchCriteria searchCriteria, out int rowsCount);

    }
}
