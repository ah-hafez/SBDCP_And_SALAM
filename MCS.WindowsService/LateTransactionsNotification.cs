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
    public partial class LateTransactionsNotification : ServiceBase
    {
        private Timer timer;
        public LateTransactionsNotification()
        {
            InitializeComponent();
        }
        private void LateTransactionsNotification_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - LateTransactionsNotification ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count} / LateTransactionsNotification----", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - Late Transactions Notification ----", LoggingCategory.Information.ToString());
                        var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/SendLateTransactionReminderToSender"),
                            AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                        if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success send reminder to user on late transaction----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Failed send reminder to user on late transaction----", LoggingCategory.Information.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - Late Transactions Notification ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----Start Call - Late Transactions Notification ----", LoggingCategory.Information.ToString());
                var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/SendLateTransactionReminderToSender"), AuthorizationApiHelper.GetAccessToken()).Result;
                if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success send reminder to user on late transaction----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed send reminder to user on late transaction----", LoggingCategory.Information.ToString());
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            if (int.TryParse(SystemSettings.TimeIntervalCheckEndTask.ToString(), out int timeInterval))
            {
                timer = new Timer(timeInterval);
                timer.Elapsed += LateTransactionsNotification_Elapsed; ;
                timer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start Late Transaction Notification----", LoggingCategory.Information.ToString());
            }
         
        }
        protected override void OnStop()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= LateTransactionsNotification_Elapsed;
                timer.Dispose();
                timer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop Late Transaction Notification----", LoggingCategory.Information.ToString());
            }        
        }
    }
}
