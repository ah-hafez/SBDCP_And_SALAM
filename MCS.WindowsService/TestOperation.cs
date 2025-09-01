using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCS.Framework.Logging;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Tenants;
using MCS.WindowsService.Helpers;
using MCS.WindowsService.Logging;
using MCS.WindowsService.Wrappers;

namespace MCS.WindowsService
{
    public partial class TestOperation : Form
    {
        public TestOperation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                    .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK)
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>
                .GetItemRequest(string.Format("api/tenant/getAllTenants"), AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK)
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - Notify By Email ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - Notify By Email  ----", LoggingCategory.Information.ToString());

                        var postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/WindowsService/TenantNotifyByEmail")
                                   , null, true, -1, AuthorizationApiHelper.GetAccessToken()).Result;
                        if (postResult.StatusCode == StatusCode.CodeOK)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - Notify By Email ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
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
                if (response.StatusCode == StatusCode.CodeOK)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success notify by email----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed notify by email----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoggerBlock.LoggerBlockValue.Write("----Start Loop - Document Migration ----", LoggingCategory.Information.ToString());
            var MigrateDocuments = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/WindowsService/MigrateDocuments?pageSize=20"),
                AuthorizationApiHelper.GetAccessToken(), false, -1).Result;
            if (MigrateDocuments.StatusCode == StatusCode.CodeOK)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success verify migration - Document Migration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed verify migration - Document Migration----", LoggingCategory.Error.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/AddEntitySync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/AddUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/UpdateEntityNameSync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DeleteUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveEntitySync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DelegationUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

            if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed AddUserSync ERPIntegration----", LoggingCategory.Error.ToString());
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - UserReminderBeforeTaskEnded ----", LoggingCategory.Information.ToString());
            
            LoggerBlock.LoggerBlockValue.Write("----Start Loop - Check End Task ----", LoggingCategory.Information.ToString());
            var CheckEndTasks = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/SendLateTransactionWithNotifyLetterTypes"),
                AuthorizationApiHelper.GetAccessToken(), false, 0, string.Empty).Result;
            if (CheckEndTasks.StatusCode == StatusCode.CodeOK)
            {
                LoggerBlock.LoggerBlockValue.Write("----Success send reminder to user before task ended----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----failed send reminder to user before task ended----", LoggingCategory.Information.ToString());
            }

            LoggerBlock.LoggerBlockValue.Write("----End Loop - Check End Task ----", LoggingCategory.Information.ToString());
        }
    }
}
