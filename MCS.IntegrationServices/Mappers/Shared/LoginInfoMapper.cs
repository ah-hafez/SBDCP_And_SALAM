using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.IntegrationServices.Models;


namespace MCS.IntegrationServices.Mappers
{
    public static class LoginInfoMapper
    {
        public static List<LoginInfoDTO> Map(IList<LoginInfoVM> loginInfoVMs)
        {
            if (loginInfoVMs == null || !loginInfoVMs.Any())
            {
                return new List<LoginInfoDTO>();
            }
            List<LoginInfoDTO> loginInfoDTOs = loginInfoVMs
                .Select(b => new LoginInfoDTO
                {
                    IsWindowsLogin = b.IsWindowsLogin,
                    Password = b.Password, //Already Encrypted 
                    RememberMe = b.RememberMe,
                    UserName = b.UserName
                }).ToList();
            return loginInfoDTOs;

        }
        public static List<LoginInfoVM> Map(IList<LoginInfoDTO> loginInfoDTOs)
        {
            if (loginInfoDTOs == null || !loginInfoDTOs.Any())
            {
                return new List<LoginInfoVM>();
            }
            List<LoginInfoVM> loginInfoVMs = loginInfoDTOs
                .Select(b => new LoginInfoVM
                {
                    IsWindowsLogin = b.IsWindowsLogin,
                    RememberMe = b.RememberMe,
                    UserName = b.UserName
                }).ToList();
            return loginInfoVMs;

        }
        public static LoginInfoVM Map(LoginInfoDTO loginInfoDTO)
        {
            if (loginInfoDTO != null)
            {
                LoginInfoVM loginInfoVM = new LoginInfoVM()
                {
                    IsWindowsLogin = loginInfoDTO.IsWindowsLogin,
                    RememberMe = loginInfoDTO.RememberMe,
                    UserName = loginInfoDTO.UserName
                };
                return loginInfoVM;
            }
            return new LoginInfoVM();
        }
        public static LoginInfoDTO Map(LoginInfoVM loginInfoVM)
        {
            if (loginInfoVM != null)
            {
                LoginInfoDTO loginInfoDTO = new LoginInfoDTO()
                {
                    IsWindowsLogin = loginInfoVM.IsWindowsLogin,
                    Password = loginInfoVM.Password,
                    RememberMe = loginInfoVM.RememberMe,
                    UserName = loginInfoVM.UserName
                };
                return loginInfoDTO;
            }
            return new LoginInfoDTO();
        }
    }
}