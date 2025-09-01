using MCS.Domain;
using System.Collections.Generic;

namespace MCS.DataAccess
{
    public interface IOnlineUserRepository : IRepository<OnlineUser>
    {
        bool AddUserOnline(int userId, int OrgUnitId, string connectionId);
        bool DeleteOnlineUser(string connectionId);
        bool UpdateUserOnline(int userId, int OrgUnitId);
        List<OnlineUser> GetOnlineUser();
    }
}
