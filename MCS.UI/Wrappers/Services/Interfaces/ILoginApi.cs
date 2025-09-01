using Refit;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.UI.Wrappers
{
    public interface ILoginApi
    {
        [Post("/api/Login/Login")]
        Task<PostObjectResult<UserDTO>> Login(LoginInfoDTO loginInfoDTO, string cultureName);

        [Post("/api/Login/ExternalLogin")]
        Task<PostObjectResult<UserDTO>> ExternalLogin(string cultureName);

        [Post("/api/Login/Logout")]
        Task<PostResult> Logout();

        [Post("/api/Login/ResetPasswordStepOne")]
        Task<PostResult> ResetPasswordStepOne(ResetPasswordDTO resetPasswordDTO, string cultureName, string resetPasswordUrl);

        [Post("/api/Login/ResetPasswordStepTwo")]
        Task<PostResult> ResetPasswordStepTwo(ResetPasswordDTO resetPasswordDTO);

    }
}
