using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Business.ASPNETIdentity.Configuration
{
    public class MultiTenantCustomUserManagerConfiguration : ConfigurationSection
    {
        #region Properties
        [ConfigurationProperty("EmailBodyFormat", DefaultValue = "Your Email Body is {0}")]
        public string EmailBodyFormat
        {
            get
            {
                return (string)this["EmailBodyFormat"];
            }
        }
        [ConfigurationProperty("PhoneNumberMessageFormat", DefaultValue = "Your Phone Number Message is {0}")]
        public string PhoneNumberMessageFormat
        {
            get
            {
                return (string)this["PhoneNumberMessageFormat"];
            }
        }
        [ConfigurationProperty("RequireUniqueEmail", DefaultValue = "true")]
        public bool RequireUniqueEmail
        {
            get
            {
                return (bool)this["RequireUniqueEmail"];
            }
        }
        [ConfigurationProperty("UserLockoutEnabledByDefault", DefaultValue = "true")]
        public bool UserLockoutEnabledByDefault
        {
            get
            {
                return (bool)this["UserLockoutEnabledByDefault"];
            }
        }
        [ConfigurationProperty("AllowOnlyAlphanumericUserNames", DefaultValue = "true")]
        public bool AllowOnlyAlphanumericUserNames
        {
            get
            {
                return (bool)this["AllowOnlyAlphanumericUserNames"];
            }
        }
        [ConfigurationProperty("EmailSubject", DefaultValue = "Subject")]
        public string EmailSubject
        {
            get
            {
                return (string)this["EmailSubject"];
            }
        }
        [ConfigurationProperty("MaxFailedAccessAttemptsBeforeLockout", DefaultValue = "5")]
        public int MaxFailedAccessAttemptsBeforeLockout
        {
            get
            {
                return (int)this["MaxFailedAccessAttemptsBeforeLockout"];
            }
        }
        [ConfigurationProperty("DefaultAccountLockoutTimeSpan", DefaultValue = "5")]
        public int DefaultAccountLockoutTimeSpan
        {
            get
            {
                return (int)this["DefaultAccountLockoutTimeSpan"];
            }
        }
        [ConfigurationProperty("PasswordRequiredLength", DefaultValue = "10")]
        public int PasswordRequiredLength
        {
            get
            {
                return (int)this["PasswordRequiredLength"];
            }
        }
        [ConfigurationProperty("PasswordRequireNonLetterOrDigit", DefaultValue = "true")]
        public bool PasswordRequireNonLetterOrDigit
        {
            get
            {
                return (bool)this["PasswordRequireNonLetterOrDigit"];
            }
        }
        [ConfigurationProperty("PasswordRequireDigit", DefaultValue = "true")]
        public bool PasswordRequireDigit
        {
            get
            {
                return (bool)this["PasswordRequireDigit"];
            }
        }
        [ConfigurationProperty("PasswordRequireLowercase", DefaultValue = "true")]
        public bool PasswordRequireLowercase
        {
            get
            {
                return (bool)this["PasswordRequireLowercase"];
            }
        }
        [ConfigurationProperty("PasswordRequireUppercase", DefaultValue = "true")]
        public bool PasswordRequireUppercase
        {
            get
            {
                return (bool)this["PasswordRequireUppercase"];
            }
        }
        [ConfigurationProperty("CustomizePasswordValidationLength", DefaultValue = "5")]
        public int CustomizePasswordValidationLength
        {
            get
            {
                return (int)this["CustomizePasswordValidationLength"];
            }
        }
        [ConfigurationProperty("PhoneCode", DefaultValue = "Phone Code")]
        public string PhoneCode
        {
            get
            {
                return (string)this["PhoneCode"];
            }
        }
        [ConfigurationProperty("EmailCode", DefaultValue = "Email Code")]
        public string EmailCode
        {
            get
            {
                return (string)this["EmailCode"];
            }
        }
        #endregion Properties
    }
}
