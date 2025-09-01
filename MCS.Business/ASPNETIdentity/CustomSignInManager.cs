using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MCS.Framework.Security;
using MCS.Common;

namespace MCS.Business.ASPNETIdentity
{
    public class CustomSignInManager : SignInManager<ASPNetIdentityUser, string>, ICustomSignInManager
    {
        public CustomSignInManager(UserManager<ASPNetIdentityUser> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }

        public static CustomSignInManager Create(IdentityFactoryOptions<CustomSignInManager> options, IOwinContext context)
        {
            return new CustomSignInManager(context.GetUserManager<CustomUserManager>(), context.Authentication);
        }

        public bool GenerateUser(IApplicationUser applicationUser, string password, out string identityId)
        {
            ASPNetIdentityUser user = applicationUser as ASPNetIdentityUser;

            user.EmailConfirmed = false;

            IdentityResult result = UserManager.Create(user, password);

            identityId = user.Id;

            return result.Succeeded;
        }

        public bool AddUserLogin(string userIdentity, string providerName, string providerKey)
        {
            UserLoginInfo userLoginInfo = new UserLoginInfo(providerName, providerKey);

            IdentityResult result = UserManager.AddLogin(userIdentity, userLoginInfo);

            return result.Succeeded;
        }

        public IApplicationUser GetUser(string userIdentity)
        {
            return UserManager.FindById(userIdentity);
        }

        public void UpdateUser(IApplicationUser applicationUser)
        {
            ASPNetIdentityUser user = applicationUser as ASPNetIdentityUser;

            UserManager.Update(user);
        }

        public void AddClaim(string userId, Claim claim)
        {
            IdentityResult result = UserManager.AddClaim(userId, claim);
        }

        public void RemoveClaim(string userId, Claim claim)
        {
            IdentityResult result = UserManager.RemoveClaim(userId, claim);
        }

        public bool CheckPassword(string userId, string password)
        {
            ASPNetIdentityUser identityUser = UserManager.FindById(userId);

            return UserManager.CheckPassword(identityUser, password);
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword, out IEnumerable<string> errors)
        {
            IdentityResult result = UserManager.ChangePassword(userId, oldPassword, newPassword);

            errors = result.Errors;

            return result.Succeeded;
        }

        public bool ResetPassword(string userId, string token, string newPassword)
        {
            IdentityResult result = UserManager.ResetPassword(userId, token, newPassword);

            bool isConfirmed = UserManager.IsEmailConfirmed(userId);

            if (!isConfirmed && result.Succeeded)
            {
                string confirmEmailToken = UserManager.GenerateEmailConfirmationToken(userId);
                UserManager.ConfirmEmail(userId, confirmEmailToken);
            }

            return result.Succeeded;
        }

        public bool ResetPassword(string userId, string token, string newPassword, string code, string phoneNumber)
        {
            ValidatePassword(newPassword);

            var resultPhone = UserManager.VerifyChangePhoneNumberToken(userId, code, phoneNumber);

            if (!resultPhone)
            {
                throw new BusinessException(StatusCode.RestCodeInvalid);
            }

            IdentityResult result = UserManager.ResetPassword(userId, token, newPassword);

            if (!result.Succeeded)
            {
                throw new BusinessException(StatusCode.ResetTokenInvalid);
            }

            bool isConfirmed = UserManager.IsEmailConfirmed(userId);

            if (!isConfirmed)
            {
                string confirmEmailToken = UserManager.GenerateEmailConfirmationToken(userId);

                UserManager.ConfirmEmail(userId, confirmEmailToken);
            }

            return true;
        }

        public string GenerateVarificationCode(string userId, string phoneNumber)
        {
            return UserManager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber).Result;
        }

        public string GenerateResetPasswordToken(string userId)
        {
            return UserManager.GeneratePasswordResetToken(userId);
        }

        public IApplicationUser Find(string userName, string password)
        {
            ASPNetIdentityUser identityUser = UserManager.Find(userName, password);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public Task<IdentityResult> ValidateAsync(string password)
        {
            return UserManager.PasswordValidator.ValidateAsync(password);
        }

        public async Task<IApplicationUser> FindAsync(string userName, string password)
        {
            ASPNetIdentityUser identityUser = await UserManager.FindAsync(userName, password);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public void RaisePasswordValidationException(string code)
        {
            if (StatusCode.PasswordRequired.ToString() == code)
                throw new BusinessException(StatusCode.PasswordRequired);

            if (StatusCode.InvalidPasswordRequiredLength.ToString() == code)
                throw new BusinessException(StatusCode.InvalidPasswordRequiredLength);

            if (StatusCode.PasswordRequireNonLetterOrDigit.ToString() == code)
                throw new BusinessException(StatusCode.PasswordRequireNonLetterOrDigit);

            if (StatusCode.PasswordRequireDigit.ToString() == code)
                throw new BusinessException(StatusCode.PasswordRequireDigit);

            if (StatusCode.PasswordRequireLowercase.ToString() == code)
                throw new BusinessException(StatusCode.PasswordRequireLowercase);

            if (StatusCode.PasswordRequireUppercase.ToString() == code)
                throw new BusinessException(StatusCode.PasswordRequireUppercase);
        }

        public async Task<IApplicationUser> FindByNameAsync(string userName)
        {
            ASPNetIdentityUser identityUser = await UserManager.FindByNameAsync(userName);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public async Task<IApplicationUser> FindAsync(ExternalUserLoginInfo externalUserLoginInfo)
        {
            UserLoginInfo userLoginInfo = new
                UserLoginInfo(externalUserLoginInfo.LoginProvider, externalUserLoginInfo.ProviderKey);

            ASPNetIdentityUser identityUser = await UserManager.FindAsync(userLoginInfo);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public IApplicationUser FindByName(string userName)
        {
            ASPNetIdentityUser identityUser = UserManager.FindByName(userName);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public IApplicationUser FindByEmail(string email)
        {
            ASPNetIdentityUser identityUser = UserManager.FindByEmail(email);

            if (identityUser != null)
            {
                ((IApplicationUser)identityUser).Id = identityUser.Id;
            }

            return identityUser;
        }

        public void SendEmail(string userId, string subject, string body)
        {
            UserManager.SendEmail(userId, subject, body);
        }

        public void SendSMS(string userId, string message)
        {
            UserManager.SendSms(userId, message);
        }

        public IList<Claim> Claims(string userId)
        {
            return UserManager.GetClaims(userId);
        }

        public SignInStatus GetSignInStatus(string userName, string password)
        {
            return this.PasswordSignIn(userName, password, false, false);
        }

        public bool SignIn(string userName, string password)
        {
            SignInStatus signInStatus = this.PasswordSignIn(userName, password, false, false);

            return (signInStatus == SignInStatus.Success);
        }

        public bool SignIn(string userName, params object[] extraParams)
        {
            if (extraParams == null || extraParams.Length == 0)
            {
                throw new ArgumentNullException("ExternalLoginInfo cannot be null");
            }

            ExternalLoginInfo externalLoginInfo = new ExternalLoginInfo();

            externalLoginInfo.DefaultUserName = userName;


            SignInStatus signInStatus = this.ExternalSignIn(externalLoginInfo, false);

            return (signInStatus == SignInStatus.Success);
        }

        public async Task<ClaimsIdentity> GenerateIdentityAsync(IApplicationUser user, string authenticationType)
        {
            ASPNetIdentityUser identityUser = user as ASPNetIdentityUser;

            var userIdentity = await UserManager.CreateIdentityAsync(identityUser, authenticationType);

            return userIdentity;
        }

        public bool CheckUserEmailConfirmed(string userId)
        {
            return UserManager.IsEmailConfirmed(userId);
        }

        private void ValidatePassword(string password)
        {
            bool isPasswordValid = UserManager.PasswordValidator.ValidateAsync(password).Result.Succeeded;

            if (!isPasswordValid)
            {
                IEnumerable<string> errorCodes = UserManager.PasswordValidator.ValidateAsync(password).Result.Errors;

                foreach (string errorCode in errorCodes)
                {
                    RaisePasswordValidationException(errorCode);
                }
            }
        }
    }
}
