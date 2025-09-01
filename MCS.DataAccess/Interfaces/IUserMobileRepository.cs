using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IUserMobileRepository
    {
        UserMobile GetUserMobile(int? userId, string userName, string cultureName);
        void UpdateUserMobile(UserMobile userMobile, string cultureName);
        void SetDefaultEntity(int userId, int defaultEntityId);
    }
}