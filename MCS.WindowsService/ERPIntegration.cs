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
    public partial class ERPIntegration : ServiceBase
    {
        private Timer AddUserSyncTimer;
        private Timer DeleteUserSyncTimer;
        private Timer MoveUserSyncTimer;
        private Timer DelegationUserSyncTimer;

        private Timer AddEntitySyncTimer;
        private Timer MoveEntitySyncTimer;
        private Timer UpdateEntityNameSyncTimer;

        public ERPIntegration()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            if (double.TryParse(SystemSettings.TimeIntervalAddUserERPIntegration.ToString(), out double addUserSyncTimeInterval))
            {
                AddUserSyncTimer = new Timer(addUserSyncTimeInterval);
                AddUserSyncTimer.Elapsed += AddUserSync_OnElapsedTime;
                AddUserSyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (double.TryParse(SystemSettings.TimeIntervalDeleteUserERPIntegration.ToString(), out double deleteUserSyncTimeInterval))
            {
                DeleteUserSyncTimer = new Timer(deleteUserSyncTimeInterval);
                DeleteUserSyncTimer.Elapsed += DeleteUserSync_OnElapsedTime;
                DeleteUserSyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start DeleteUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (double.TryParse(SystemSettings.TimeIntervalMoveUserERPIntegration.ToString(), out double moveUserSyncTimeInterval))
            {
                MoveUserSyncTimer = new Timer(moveUserSyncTimeInterval);
                MoveUserSyncTimer.Elapsed += MoveUserSync_OnElapsedTime;
                MoveUserSyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start MoveUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }

            if (double.TryParse(SystemSettings.TimeIntervalAddEntityERPIntegration.ToString(), out double addEntitySyncTimeInterval))
            {
                AddEntitySyncTimer = new Timer(addEntitySyncTimeInterval);
                AddEntitySyncTimer.Elapsed += AddEntitySync_OnElapsedTime;
                AddEntitySyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start AddEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (double.TryParse(SystemSettings.TimeIntervalMoveEntityERPIntegration.ToString(), out double moveEntitySyncTimeInterval))
            {
                MoveEntitySyncTimer = new Timer(moveEntitySyncTimeInterval);
                MoveEntitySyncTimer.Elapsed += MoveEntitySync_OnElapsedTime;
                MoveEntitySyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start MoveEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (double.TryParse(SystemSettings.TimeIntervalUpdateEntityNameERPIntegration.ToString(), out double updateEntitySyncTimeInterval))
            {
                UpdateEntityNameSyncTimer = new Timer(updateEntitySyncTimeInterval);
                UpdateEntityNameSyncTimer.Elapsed += UpdateEntityNameSync_OnElapsedTime;
                UpdateEntityNameSyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start UpdateEntityNameSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (double.TryParse(SystemSettings.TimeIntervalDelegationERPIntegration.ToString(), out double delegationUserSyncTimeInterval))
            {
                UpdateEntityNameSyncTimer = new Timer(delegationUserSyncTimeInterval);
                UpdateEntityNameSyncTimer.Elapsed += DelegationUserSync_OnElapsedTime;
                UpdateEntityNameSyncTimer.Start();
                LoggerBlock.LoggerBlockValue.Write("----Start DelegationUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
        }
        protected override void OnStop()
        {
            if (AddUserSyncTimer != null)
            {
                AddUserSyncTimer.Stop();
                AddUserSyncTimer.Elapsed -= AddUserSync_OnElapsedTime;
                AddUserSyncTimer.Dispose();
                AddUserSyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop AddUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (DeleteUserSyncTimer != null)
            {
                DeleteUserSyncTimer.Stop();
                DeleteUserSyncTimer.Elapsed -= DeleteUserSync_OnElapsedTime;
                DeleteUserSyncTimer.Dispose();
                DeleteUserSyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop DeleteUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (MoveUserSyncTimer != null)
            {
                MoveUserSyncTimer.Stop();
                MoveUserSyncTimer.Elapsed -= MoveUserSync_OnElapsedTime;
                MoveUserSyncTimer.Dispose();
                MoveUserSyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop MoveUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (DelegationUserSyncTimer != null)
            {
                DelegationUserSyncTimer.Stop();
                DelegationUserSyncTimer.Elapsed -= DelegationUserSync_OnElapsedTime;
                DelegationUserSyncTimer.Dispose();
                DelegationUserSyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop DelegationUserSync ERPIntegration----", LoggingCategory.Information.ToString());
            }

            if (AddEntitySyncTimer != null)
            {
                AddEntitySyncTimer.Stop();
                AddEntitySyncTimer.Elapsed -= AddEntitySync_OnElapsedTime;
                AddEntitySyncTimer.Dispose();
                AddEntitySyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop AddEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (MoveEntitySyncTimer != null)
            {
                MoveEntitySyncTimer.Stop();
                MoveEntitySyncTimer.Elapsed -= MoveEntitySync_OnElapsedTime;
                MoveEntitySyncTimer.Dispose();
                MoveEntitySyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop MoveEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
            }
            if (UpdateEntityNameSyncTimer != null)
            {
                UpdateEntityNameSyncTimer.Stop();
                UpdateEntityNameSyncTimer.Elapsed -= UpdateEntityNameSync_OnElapsedTime;
                UpdateEntityNameSyncTimer.Dispose();
                UpdateEntityNameSyncTimer = null;
                LoggerBlock.LoggerBlockValue.Write("----Stop UpdateEntityNameSync ERPIntegration----", LoggingCategory.Information.ToString());
            }
        }
        private void AddUserSync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - AddUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - AddUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/AddUserSync"),
                                                    AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                        if (result.StatusCode == StatusCode.CodeOK|| result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - AddUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - AddUserSync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - AddUserSync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - AddUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - AddUserSync ----", LoggingCategory.Error.ToString());
                }
            }
            else
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
        }

        private void DeleteUserSync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - DeleteUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - DeleteUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DeleteUserSync"),
                                AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;
                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - DeleteUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - DeleteUserSync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - DeleteUserSync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - DeleteUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - DeleteUserSync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DeleteUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success DeleteUserSync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed DeleteUserSync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void MoveUserSync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - MoveUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - MoveUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveUserSync"),
                                AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;

                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - MoveUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - MoveUserSync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - MoveUserSync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - MoveUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - MoveUserSync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success MoveUserSync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed MoveUserSync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void DelegationUserSync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - DelegationUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - DelegationUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DelegationUserSync"),
                                AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;

                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - DelegationUserSync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - DelegationUserSync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - DelegationUserSync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - DelegationUserSync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - DelegationUserSync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/DelegationUserSync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success DelegationUserSync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed DelegationUserSync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void AddEntitySync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - AddEntitySync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - AddEntitySync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/AddEntitySync"),
                                AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;

                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - AddEntitySync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - AddEntitySync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - AddEntitySync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - AddEntitySync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - AddEntitySync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/AddEntitySync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success AddEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed AddEntitySync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void MoveEntitySync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - MoveEntitySync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - MoveEntitySync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveEntitySync"),
                                AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;

                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - MoveEntitySync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - MoveEntitySync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - MoveEntitySync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - MoveEntitySync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - MoveEntitySync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/MoveEntitySync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success MoveEntitySync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed MoveEntitySync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }

        private void UpdateEntityNameSync_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            if (SystemConfigurations.MultiTenantEnabled)
            {
                GetResult<List<TenantDTO>> Tenants = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest(string.Format("api/tenant/getAllTenants"),
                    AuthorizationApiHelper.GetAccessToken(), true).Result;
                if (Tenants.StatusCode == StatusCode.CodeOK || Tenants.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Get All Tenants - - UpdateEntityNameSync ERPIntegration ----", LoggingCategory.Information.ToString());
                    LoggerBlock.LoggerBlockValue.Write($"----Tenant Count: {Tenants.Result.Count}---- - CheckEndTasks", LoggingCategory.Information.ToString());
                    foreach (var item in Tenants.Result)
                    {
                        LoggerBlock.LoggerBlockValue.Write("----Start Loop - UpdateEntityNameSync ERPIntegration  ----", LoggingCategory.Information.ToString());

                        var result = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/UpdateEntityNameSync"),
                            AuthorizationApiHelper.GetAccessToken(), false, item.Id, item.DatabaseName).Result;

                        if (result.StatusCode == StatusCode.CodeOK || result.StatusCode == StatusCode.Ok)
                        {
                            LoggerBlock.LoggerBlockValue.Write("----Success verify - UpdateEntityNameSync ERPIntegration ----", LoggingCategory.Information.ToString());
                        }
                        else
                        {
                            LoggerBlock.LoggerBlockValue.Write($"----failed verify ended - UpdateEntityNameSync ERPIntegration - statusCode {result.StatusCode}----", LoggingCategory.Error.ToString());
                            LoggerBlock.LoggerBlockValue.Write("----failed verify ended - UpdateEntityNameSync ERPIntegration ----", LoggingCategory.Error.ToString());
                        }
                    }
                    LoggerBlock.LoggerBlockValue.Write("----End Loop - UpdateEntityNameSync ERPIntegration  ----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----Error occur when get all Tenants - UpdateEntityNameSync ----", LoggingCategory.Error.ToString());
                }
            }
            else
            {
                var response = HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/windowsService/UpdateEntityNameSync"), AuthorizationApiHelper.GetAccessToken()).Result;

                if (response.StatusCode == StatusCode.CodeOK || response.StatusCode == StatusCode.Ok)
                {
                    LoggerBlock.LoggerBlockValue.Write("----Success UpdateEntityNameSync ERPIntegration----", LoggingCategory.Information.ToString());
                }
                else
                {
                    LoggerBlock.LoggerBlockValue.Write("----failed UpdateEntityNameSync ERPIntegration----", LoggingCategory.Error.ToString());
                }
            }
        }
    }
}
