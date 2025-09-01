using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MCS.Framework.Security
{
    public interface ICustomSignInManager
    {
        bool GenerateUser(IApplicationUser user, string password, out string identityId);
        bool AddUserLogin(string userIdentity, string providerName, string providerKey);
        void UpdateUser(IApplicationUser user);
        bool SignIn(string userName, string password);
        bool SignIn(string userName, params object[] extraParams);
        void AddClaim(string userId, Claim claim);
        void RemoveClaim(string userId, Claim claim);
        bool CheckPassword(string userName, string password);
        string GenerateResetPasswordToken(string userId);
        string GenerateVarificationCode(string userId, string phoneNumber);
        bool ResetPassword(string userId, string token, string newPassword);
        bool ResetPassword(string userId, string token, string newPassword, string code, string phoneNumber);
        bool ChangePassword(string userId, string oldPassword, string newPassword, out IEnumerable<string> errors);
        IApplicationUser Find(string userName, string password);
        IApplicationUser GetUser(string userIdentity);
        Task<IApplicationUser> FindAsync(string userName, string password);
        Task<IApplicationUser> FindAsync(ExternalUserLoginInfo externalUserLoginInfo);
        Task<IApplicationUser> FindByNameAsync(string userName);
        IApplicationUser FindByName(string userName);
        IApplicationUser FindByEmail(string email);
        void SendEmail(string userId, string subject, string body);
        void SendSMS(string userId, string message);
        IList<Claim> Claims(string userId);
        Task<ClaimsIdentity> GenerateIdentityAsync(IApplicationUser user, string authenticationType);
        bool CheckUserEmailConfirmed(string userId);
    }
}
