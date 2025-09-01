using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using MCS.Business.ASPNETIdentity.Configuration;

namespace MCS.Business.ASPNETIdentity
{
    public class CustomUserManager : UserManager<ASPNetIdentityUser>
    {
        public CustomUserManager(IUserStore<ASPNetIdentityUser> store)
            : base(store)
        {

        }

        public static CustomUserManager Create(IdentityFactoryOptions<CustomUserManager> options, IOwinContext context)
        {

            var manager = new CustomUserManager(new UserStore<ASPNetIdentityUser>(context.Get<CustomIdentityDbContext>()));

            manager.UserValidator = new UserValidator<ASPNetIdentityUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = SecurityConfigurationManager.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = SecurityConfigurationManager.RequireUniqueEmail
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new CustomizePasswordValidation(SecurityConfigurationManager.CustomizePasswordValidationLength)
            {
                RequiredLength = SecurityConfigurationManager.PasswordRequiredLength,
                RequireNonLetterOrDigit = SecurityConfigurationManager.PasswordRequireNonLetterOrDigit,
                RequireDigit = SecurityConfigurationManager.PasswordRequireDigit,
                RequireLowercase = SecurityConfigurationManager.PasswordRequireLowercase,
                RequireUppercase = SecurityConfigurationManager.PasswordRequireUppercase,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = SecurityConfigurationManager.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(SecurityConfigurationManager.DefaultAccountLockoutTimeSpan);
            manager.MaxFailedAccessAttemptsBeforeLockout = SecurityConfigurationManager.MaxFailedAccessAttemptsBeforeLockout;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.

            manager.RegisterTwoFactorProvider(SecurityConfigurationManager.PhoneCode, new PhoneNumberTokenProvider<ASPNetIdentityUser>

            {
                MessageFormat = SecurityConfigurationManager.PhoneNumberMessageFormat
            });
            manager.RegisterTwoFactorProvider(SecurityConfigurationManager.EmailCode, new EmailTokenProvider<ASPNetIdentityUser>
            {
                Subject = SecurityConfigurationManager.EmailSubject,
                BodyFormat = SecurityConfigurationManager.EmailBodyFormat
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ASPNetIdentityUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
