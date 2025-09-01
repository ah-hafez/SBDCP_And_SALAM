using System.Collections.Generic;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IUserPendingGroupBL
    {
  
        int RequestRoleItem(UserPendingGroup userPendingGroup, string cultureName);

        List<UserPendingGroup> GetuserPendingGroup(string cultureName);

        UserGroup ApproveRoleRequest(int Id , string CultureName);

        bool RejectRoleRequest(int Id);
        List<UserPendingGroup> GetuserPendingRequest(string cultureName);
        bool ApproveManagerRoleRequest(int Id);
        bool RejectManagerRoleRequest(int Id);



    }
}
