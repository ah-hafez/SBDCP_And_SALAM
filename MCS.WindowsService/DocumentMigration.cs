using System;
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
    public partial class DocumentMigration : ServiceBase
    {
        private Timer MigrationTimer;
        private string ScheduledTime;
        private int MigratePageSize = 20;

        public DocumentMigration()
        {
            InitializeComponent();
        }
        private void MigrationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(10000);

                if (SystemConfigurations.MultiTenantEnabled)
                {
                    GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                        .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                    if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - Document Migration ----", LoggingCategory.Information.ToString());
                        LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - Document Migration", LoggingCategory.Information.ToString());
                        foreach (var item in Tenants.Result)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Start Loop - Document Migration ----", LoggingCategory.Information.ToString());

                            int documentsCount = 0;
                            do
                            {
                                var MigrateDocuments = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/WindowsService/MigrateDocuments?pageSize=" + MigratePageSize),
                                                        AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                                documentsCount = MigrateDocuments.Result;

                                if (MigrateDocuments.StatusCode == StatusCode.CodeOK)
                                {
                                    LoggerBlock.LoggerBlockValue.Write("----Success verify migration - Document Migration----", LoggingCategory.Information.ToString());
                                }
                                else
                                {
                                    LoggerBlock.LoggerBlockValue.Write("----failed verify migration - Document Migration----", LoggingCategory.Error.ToString());
                                }
                            } while (documentsCount > 0);


                        }
                        LoggerBlock.LoggerBlockValue.Write("----End Loop - Document Migration ----", LoggingCategory.Information.ToString());
                    }
                    else
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants----", LoggingCategory.Error.ToString());
                    }
                }
                else
                {
                    int documentsCount = 0;
                    do
                    {
                        var MigrateDocuments = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/WindowsService/MigrateDocuments?pageSize=" + MigratePageSize),
                                                AuthorizationApiHelper.GetAccessToken()).Result;
                        documentsCount = MigrateDocuments.Result;

                        if (MigrateDocuments.StatusCode == StatusCode.CodeOK)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify migration - Document Migration----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write("----failed verify migration - Document Migration----", LoggingCategory.Error.ToString());
                        }
                    } while (documentsCount > 0);
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.LoggerBlockValue.Write("----Error Exception----" + ex.Message, LoggingCategory.Error.ToString());
            }
        }

        protected override void OnStart(string[] args)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["MigrateScheduledTime"] != null && !string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["MigrateScheduledTime"].ToString()))
            {
                //if (System.Configuration.ConfigurationManager.AppSettings["MigratePageSize"] != null 
                //    && !string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["MigratePageSize"].ToString()))
                //{
                //    MigratePageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MigratePageSize"].ToString());
                //}

                //ScheduledTime = System.Configuration.ConfigurationManager.AppSettings["MigrateScheduledTime"].ToString();
                ////Set the Default Time.
                //DateTime scheduledTime = DateTime.MinValue;

                ////Get the Scheduled Time from AppSettings.
                //scheduledTime = DateTime.Parse(ScheduledTime);
                //if (DateTime.Now > scheduledTime)
                //{
                //    //If Scheduled Time is passed set Schedule for the next day.
                //    scheduledTime = scheduledTime.AddDays(1);
                //}

                //TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                //string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                //LoggerBlock.LoggerBlockValue.Write("Service Document Migration scheduled to run after: " + schedule);

                if (double.TryParse(SystemSettings.TimeIntervalNotifyEmail.ToString(), out double emailTimeInterval))
                {
                    MigrationTimer = new Timer(emailTimeInterval);
                    MigrationTimer.Elapsed += MigrationTimer_Elapsed;
                    MigrationTimer.Start();
                    LoggerBlock.LoggerBlockValue.Write("----Start Document Migration----", LoggingCategory.Information.ToString());
                }
            }
        }
        protected override void OnStop()
        {
            if (MigrationTimer != null)
            {
                MigrationTimer.Stop();
                MigrationTimer.Elapsed -= MigrationTimer_Elapsed;
                MigrationTimer.Dispose();
                MigrationTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop Document Migration----", LoggingCategory.Information.ToString());
            }
        }
    }
}
