using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.DataAccess
{
    public interface IUserPendingGroupsRepository : IRepository<UserPendingGroup>
    {
        int RequestRole(UserPendingGroup userPendingGroup);

        List<UserPendingGroup> GetuserPendingGroup(string cultureName);

        UserGroup ApproveRoleRequest(int Id, string CultureName);

        bool RejectRoleRequest(int Id);
        List<UserPendingGroup> GetuserPendingRequest(string cultureName, int userId);
        bool ApproveManagerRoleRequest(int Id);
        bool RejectManagerRoleRequest(int Id);
    }
}
