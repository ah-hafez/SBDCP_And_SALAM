using System.Configuration;

namespace MCS.Business.ASPNETIdentity.Configuration
{
    public class SecurityConfigurationManager
    {
        #region Attributes

        public static string PhoneNumberMessageFormat;
        public static string EmailBodyFormat;
        public static bool RequireUniqueEmail;
        public static bool UserLockoutEnabledByDefault;
        public static bool AllowOnlyAlphanumericUserNames;
        public static string EmailSubject;
        public static int MaxFailedAccessAttemptsBeforeLockout;
        public static int DefaultAccountLockoutTimeSpan;
        public static int PasswordRequiredLength;
        public static bool PasswordRequireNonLetterOrDigit;
        public static bool PasswordRequireDigit;
        public static bool PasswordRequireLowercase;
        public static bool PasswordRequireUppercase;
        public static int CustomizePasswordValidationLength;
        public static string PhoneCode;
        public static string EmailCode;

        #endregion Attributes

        #region Methods

        static SecurityConfigurationManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            CustomUserManagerConfiguration configSection = (CustomUserManagerConfiguration)ConfigurationManager.GetSection("CustomUserManagerSectionGroup/CustomUserManager");

            if (configSection == null)
                throw new ConfigurationErrorsException("CustomUserManagerSectionGroup section is not set.");

            EmailBodyFormat = configSection.EmailBodyFormat;
            RequireUniqueEmail = configSection.RequireUniqueEmail;
            UserLockoutEnabledByDefault = configSection.UserLockoutEnabledByDefault;
            AllowOnlyAlphanumericUserNames = configSection.AllowOnlyAlphanumericUserNames;
            EmailSubject = configSection.EmailSubject;
            MaxFailedAccessAttemptsBeforeLockout = configSection.MaxFailedAccessAttemptsBeforeLockout;
            DefaultAccountLockoutTimeSpan = configSection.DefaultAccountLockoutTimeSpan;
            PhoneNumberMessageFormat = configSection.PhoneNumberMessageFormat;
            PasswordRequiredLength = configSection.PasswordRequiredLength;
            PasswordRequireNonLetterOrDigit = configSection.PasswordRequireNonLetterOrDigit;
            PasswordRequireDigit = configSection.PasswordRequireDigit;
            PasswordRequireLowercase = configSection.PasswordRequireLowercase;
            PasswordRequireUppercase = configSection.PasswordRequireUppercase;
            CustomizePasswordValidationLength = configSection.CustomizePasswordValidationLength;
            PhoneCode = configSection.PhoneCode;
            EmailCode = configSection.EmailCode;

        }

        #endregion Methods
    }
}
