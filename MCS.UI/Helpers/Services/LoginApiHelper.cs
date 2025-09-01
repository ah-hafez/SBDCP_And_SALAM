using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class LoginApiHelper
    {
        public ILoginApi LoginApiClient { get; }
        public LoginApiHelper()
        {
            LoginApiClient = ClientFactory.GetClient<ILoginApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<PostObjectResult<UserDTO>> Login(LoginInfoDTO loginInfoDTO, string cultureName)
        {
            var client = new LoginApiHelper();
            var result = await client.LoginApiClient.Login(loginInfoDTO, cultureName);
            return result;
        }
        public static async Task<PostObjectResult<UserDTO>> ExternalLogin(string cultureName)
        {
            var client = new LoginApiHelper();
            var result = await client.LoginApiClient.ExternalLogin(cultureName);
            return result;
        }

        public static async Task<PostResult> Logout()
        {
            var client = new LoginApiHelper();
            var result = await client.LoginApiClient.Logout();
            return result;
        }
        public static async Task<PostResult> ResetPasswordStepOne(ResetPasswordDTO resetPasswordDTO, string cultureName, string resetPasswordUrl)
        {
            var client = new LoginApiHelper();
            var result = await client.LoginApiClient.ResetPasswordStepOne(resetPasswordDTO, cultureName, resetPasswordUrl);
            return result;
        }

        public static async Task<PostResult> ResetPasswordStepTwo(ResetPasswordDTO resetPasswordDTO)
        {
            var client = new LoginApiHelper();
            var result = await client.LoginApiClient.ResetPasswordStepTwo(resetPasswordDTO);
            return result;
        }
       
    }


}