using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using MCS.Framework.Logging;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.WindowsService.Helpers;
using MCS.WindowsService.Logging;
using MCS.WindowsService.Utility;
using MCS.WindowsService.Wrappers;

namespace MCS.WindowsService
{
    public partial class TaskExpirationCheck : ServiceBase
    {
        private Timer taskExpirationimer;
        private Timer UserReminderBeforeTaskEnded;
        public TaskExpirationCheck()
        {
            InitializeComponent();
        }
        private void TaskExpirationimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                    .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - Check End Task ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - Check End Task ----", LoggingCategory.Information.ToString());
                        var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/WindowsService/CheckEndTasks"),
                            AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                        if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify ended task - CheckEndTasks----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended task - CheckEndTasks----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - Check End Task ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----Start call - Check End Task ----", LoggingCategory.Information.ToString());
                var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/WindowsService/CheckEndTasks"), AuthorizationApiHelper.GetAccessToken()).Result;
                if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success verify ended task - CheckEndTasks----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed verify ended task - CheckEndTasks----", LoggingCategory.Error.ToString());
                }
            }
        }
        private void UserReminderBeforeTaskEnded_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - UserReminderBeforeTaskEnded ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count} / UserReminderBeforeTaskEnded----", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - Check End Task ----", LoggingCategory.Information.ToString());
                        var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/SendToUserReminderBeforeTaskEnded"),
                            AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                        if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success send reminder to user before task ended----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write("----failed send reminder to user before task ended----", LoggingCategory.Information.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - Check End Task ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----Start Call - SendToUserReminderBeforeTaskEnded ----", LoggingCategory.Information.ToString());
                var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/SendToUserReminderBeforeTaskEnded"), AuthorizationApiHelper.GetAccessToken()).Result;
                if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success send reminder to user before task ended----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed send reminder to user before task ended----", LoggingCategory.Information.ToString());
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            if (int.TryParse(SystemSettings.TimeIntervalCheckEndTask.ToString(), out int timeInterval))
            {
                taskExpirationimer = new Timer(timeInterval);
                taskExpirationimer.Elapsed += TaskExpirationimer_Elapsed; ;
                taskExpirationimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start Task Expiration----", LoggingCategory.Information.ToString());
            }
            if (int.TryParse(SystemSettings.TimeIntervalToUserReminderBeforeTaskEnded.ToString(), out int timeIntervalUserReminderBeforeTaskEnded))
            {
                UserReminderBeforeTaskEnded = new Timer(timeIntervalUserReminderBeforeTaskEnded);
                UserReminderBeforeTaskEnded.Elapsed += UserReminderBeforeTaskEnded_Elapsed;
                UserReminderBeforeTaskEnded.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start User Reminder Before Task Ended----", LoggingCategory.Information.ToString());
            }
        }
        protected override void OnStop()
        {
            if (taskExpirationimer != null)
            {
                taskExpirationimer.Stop();
                taskExpirationimer.Elapsed -= TaskExpirationimer_Elapsed;
                taskExpirationimer.Dispose();
                taskExpirationimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop Task Expiration----", LoggingCategory.Information.ToString());
            }
            if (UserReminderBeforeTaskEnded != null)
            {
                UserReminderBeforeTaskEnded.Stop();
                UserReminderBeforeTaskEnded.Elapsed -= UserReminderBeforeTaskEnded_Elapsed;
                UserReminderBeforeTaskEnded.Dispose();
                UserReminderBeforeTaskEnded = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop User Reminder Before Task Ended----", LoggingCategory.Information.ToString());
            }
        }
    }
}
