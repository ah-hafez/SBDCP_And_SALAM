using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.WindowsService.Utility
{
    public class SystemSettings
    {
        public static double TimeIntervalCheckEndTask
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalCheckEndTask"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalCheckEndTask"]));
                }
                throw new Exception("TimeIntervalCheckEndTask not configured in the web config file");
            }
        }
        public static double TimeIntervalToUserReminderBeforeTaskEnded
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalToUserReminderBeforeTaskEnded"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalToUserReminderBeforeTaskEnded"]));
                }
                throw new Exception("TimeIntervalToUserReminderBeforeTaskEnded not configured in the web config file");
            }
        }
        public static double TimeIntervalNotifyEmail
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalNotifyEmail"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalNotifyEmail"]));
                }
                throw new Exception("TimeIntervalNotifyEmail not configured in the web config file");
            }
        }
        public static double TimeIntervalAddUserERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalAddUserERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalAddUserERPIntegration"]));
                }
                throw new Exception("TimeIntervalAddUserERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalDeleteUserERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalDeleteUserERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalDeleteUserERPIntegration"]));
                }
                throw new Exception("TimeIntervalDeleteUserERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalMoveUserERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalMoveUserERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalMoveUserERPIntegration"]));
                }
                throw new Exception("TimeIntervalMoveUserERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalAddEntityERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalAddEntityERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalAddEntityERPIntegration"]));
                }
                throw new Exception("TimeIntervalAddEntityERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalMoveEntityERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalMoveEntityERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalMoveEntityERPIntegration"]));
                }
                throw new Exception("TimeIntervalMoveEntityERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalUpdateEntityNameERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalUpdateEntityNameERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalUpdateEntityNameERPIntegration"]));
                }
                throw new Exception("TimeIntervalUpdateEntityNameERPIntegration not configured in the web config file");
            }
        }
        public static double TimeIntervalDelegationERPIntegration
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["TimeIntervalDelegationERPIntegration"]))
                {
                    return Extensions.ConvertMinutesToMilliseconds(double.Parse(ConfigurationManager.AppSettings["TimeIntervalDelegationERPIntegration"]));
                }
                throw new Exception("TimeIntervalDelegationERPIntegration not configured in the web config file");
            }
        }
        public static string Username
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Username"]))
                {
                    return ConfigurationManager.AppSettings["Username"];
                }
                throw new Exception("Username not configured in the web config file");
            }
        }
        public static string Password
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Password"]))
                {
                    return ConfigurationManager.AppSettings["Password"];
                }
                throw new Exception("Password not configured in the web config file");
            }
        }
        public static string MultiTenantPassword
        {
            get
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MultiTenantPassword"]))
                {
                    return ConfigurationManager.AppSettings["MultiTenantPassword"];
                }
                throw new Exception("MultiTenantPassword not configured in the web config file");
            }
        }
        public static bool MultiTenantEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["MultiTenantEnabled"].TryGetBoolValue();
            }
        }
    }
}
