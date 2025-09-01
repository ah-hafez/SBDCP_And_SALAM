using SSS.General.Utilities;
using MCS.Framework.Logging;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.WindowsService.Logging;
using MCS.WindowsService.Utility;
using MCS.WindowsService.Wrappers;

namespace MCS.WindowsService.Helpers
{
    public static class AuthorizationApiHelper
    {
        const string encryptkey1 = "9^1b4a7[";
        const string encryptkey2 = "$/?,*.=}";
        public static string GetAccessToken()
        {
            string accessToken = string.Empty;
            string url = "api/Login/Login?cultureName=ar";
            var username = EncryptDecrypt.Decrypt(SystemSettings.Username, encryptkey1, encryptkey2);
            var password = SystemSettings.Password;//EncryptDecrypt.Decrypt(SystemSettings.Password, encryptkey1, encryptkey2);
            var loginInfoDTO = new LoginInfoDTO
            {
                UserName = username,
                Password = password
            };

            if (SystemSettings.MultiTenantEnabled)
            {
                url = "api/authorization/existingUser";
                password = EncryptDecrypt.Decrypt(SystemSettings.MultiTenantPassword, encryptkey1, encryptkey2);
                loginInfoDTO.Password = password;
            }

            PostObjectResult<ApplicationUserDTO> postResult = HttpClientWrapper<PostObjectResult<ApplicationUserDTO>>.PostRequest(url, loginInfoDTO, SystemSettings.MultiTenantEnabled).Result;

            if (postResult.StatusCode == StatusCode.CodeOK || postResult.StatusCode == StatusCode.Ok)
            {
                accessToken = postResult.Result.AccessToken;
                LoggerBlock.LoggerBlockValue.Write("----Login with username and password----", LoggingCategory.Information.ToString());
            }
            else
            {
                LoggerBlock.LoggerBlockValue.Write("----Error occur when try login----", LoggingCategory.Error.ToString());
            }

            return accessToken;
        }
    }
}
