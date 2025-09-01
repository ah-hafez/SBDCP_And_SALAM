using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using MCS.Business.ASPNETIdentity.Configuration;

namespace MCS.Business.ASPNETIdentity
{
    public class MultiTenantCustomUserManager : UserManager<ASPNetIdentityUser>
    {

        public MultiTenantCustomUserManager(IUserStore<ASPNetIdentityUser> store)
            : base(store)
        {
        }
        public static MultiTenantCustomUserManager Create(IdentityFactoryOptions<MultiTenantCustomUserManager> options, IOwinContext context)
        {
            var manager = new MultiTenantCustomUserManager(new UserStore<ASPNetIdentityUser>(context.Get<MultiTenantCustomIdentityDbContext>()));
            manager.UserValidator = new UserValidator<ASPNetIdentityUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = MultiTenantSecurityConfigurationManager.AllowOnlyAlphanumericUserNames,
                RequireUniqueEmail = MultiTenantSecurityConfigurationManager.RequireUniqueEmail
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new CustomizePasswordValidation(MultiTenantSecurityConfigurationManager.CustomizePasswordValidationLength)
            {
                RequiredLength = MultiTenantSecurityConfigurationManager.PasswordRequiredLength,
                RequireNonLetterOrDigit = MultiTenantSecurityConfigurationManager.PasswordRequireNonLetterOrDigit,
                RequireDigit = MultiTenantSecurityConfigurationManager.PasswordRequireDigit,
                RequireLowercase = MultiTenantSecurityConfigurationManager.PasswordRequireLowercase,
                RequireUppercase = MultiTenantSecurityConfigurationManager.PasswordRequireUppercase,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = MultiTenantSecurityConfigurationManager.UserLockoutEnabledByDefault;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(MultiTenantSecurityConfigurationManager.DefaultAccountLockoutTimeSpan);
            manager.MaxFailedAccessAttemptsBeforeLockout = MultiTenantSecurityConfigurationManager.MaxFailedAccessAttemptsBeforeLockout;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider(MultiTenantSecurityConfigurationManager.PhoneCode, new PhoneNumberTokenProvider<ASPNetIdentityUser>
            {
                MessageFormat = MultiTenantSecurityConfigurationManager.PhoneNumberMessageFormat
            });
            manager.RegisterTwoFactorProvider(MultiTenantSecurityConfigurationManager.EmailCode, new EmailTokenProvider<ASPNetIdentityUser>
            {
                Subject = MultiTenantSecurityConfigurationManager.EmailSubject,
                BodyFormat = MultiTenantSecurityConfigurationManager.EmailBodyFormat
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
