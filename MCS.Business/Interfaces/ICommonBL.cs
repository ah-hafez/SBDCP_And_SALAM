using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ICommonBL
    {
        IList<Culture> GetCultures();
        Culture GetCultureById(int id);
        IList<Theme> GetThemes();
        bool AddUserOnline(int userId, int OrgUnitId, string connectionId);
        bool UpdateUserOnline(int userId, int OrgUnitId);
        bool DeleteOnlineUser(string connectionId);
        List<OnlineUser> GetOnlineUser();
    }
}
