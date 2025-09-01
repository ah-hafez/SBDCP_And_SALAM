using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using MCS.Framework.Logging;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Tenants;
using MCS.WindowsService.Helpers;
using MCS.WindowsService.Logging;
using MCS.WindowsService.Utility;
using MCS.WindowsService.Wrappers;

namespace MCS.WindowsService
{
    public partial class EmailSender : ServiceBase
    {
        private Timer timer;
        public EmailSender()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            if (double.TryParse(SystemSettings.TimeIntervalNotifyEmail.ToString(), out double emailTimeInterval))
            {
                timer = new Timer(emailTimeInterval); ;
                timer.Elapsed += OnElapsedTime;
                timer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start notify by email----", LoggingCategory.Information.ToString());
            }
        }
        protected override void OnStop()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= OnElapsedTime;
                timer.Dispose();
                timer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop notify by email----", LoggingCategory.Information.ToString());
            }
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - Notify By Email ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - Notify By Email  ----", LoggingCategory.Information.ToString());

                        var postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/WindowsService/TenantNotifyByEmail")
                                   , null, false, -1, AuthorizationApiHelper.GetAccessToken()).Result;
                        if (postResult.StatusCode == StatusCode.CodeOK|| postResult.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - Notify By Email ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - Notify By Email - statusCode {postResult.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - Notify By Email ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - Notify By Email  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.PostRequest(string.Format("api/WindowsService/NotifyByEmail")
                                , null, false, -1, AuthorizationApiHelper.GetAccessToken()).Result;
                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success notify by email----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed notify by email----", LoggingCategory.Error.ToString());
                }
            }
        }
    }
}
