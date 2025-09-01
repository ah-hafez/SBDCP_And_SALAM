using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public static class ResetPasswordMapper
    {
        public static List<ResetPasswordDTO> Map(IList<ResetPasswordVM> resetPasswordVMs)
        {
            if (resetPasswordVMs == null || !resetPasswordVMs.Any())
            {
                return new List<ResetPasswordDTO>();
            }
            List<ResetPasswordDTO> resetPasswordDTOs = resetPasswordVMs
                .Select(b => new ResetPasswordDTO
                {
                    Code = b.Code,
                    ConfirmPassword = b.ConfirmPassword, //Already Encrypted
                    IdentityId = b.IdentityId,
                    NewPassword = b.NewPassword,  //Already Encrypted
                    PhoneNumber = b.PhoneNumber,
                    Token = b.Token,
                    Email = b.Email,
                    UserName = b.UserName
                }).ToList();
            return resetPasswordDTOs;

        }
        public static List<ResetPasswordVM> Map(IList<ResetPasswordDTO> resetPasswordDTOs)
        {
            if (resetPasswordDTOs == null || !resetPasswordDTOs.Any())
            {
                return new List<ResetPasswordVM>();
            }
            List<ResetPasswordVM> resetPasswordVMs = resetPasswordDTOs
                .Select(b => new ResetPasswordVM
                {
                    Code = b.Code,
                    ConfirmPassword = b.ConfirmPassword,//Already Encrypted
                    IdentityId = b.IdentityId,
                    NewPassword = b.NewPassword,//Already Encrypted
                    PhoneNumber = b.PhoneNumber,
                    Token = b.Token,
                    Email = b.Email,
                    UserName = b.UserName
                }).ToList();
            return resetPasswordVMs;

        }
        public static ResetPasswordVM Map(ResetPasswordDTO b)
        {
            if (b != null)
            {
                ResetPasswordVM resetPasswordVM = new ResetPasswordVM()
                {
                    Code = b.Code,
                    ConfirmPassword = b.ConfirmPassword,//Already Encrypted
                    IdentityId = b.IdentityId,
                    NewPassword = b.NewPassword,//Already Encrypted
                    PhoneNumber = b.PhoneNumber,
                    Token = b.Token,
                    Email = b.Email,
                    UserName = b.UserName
                };
                return resetPasswordVM;
            }
            return new ResetPasswordVM();
        }
        public static ResetPasswordDTO Map(ResetPasswordVM b)
        {
            if (b != null)
            {
                ResetPasswordDTO resetPasswordDTO = new ResetPasswordDTO()
                {
                    Code = b.Code,
                    ConfirmPassword = b.ConfirmPassword,//Already Encrypted
                    IdentityId = b.IdentityId,
                    NewPassword = b.NewPassword,//Already Encrypted
                    PhoneNumber = b.PhoneNumber,
                    Token = b.Token,
                    Email = b.Email,
                    UserName = b.UserName
                };
                return resetPasswordDTO;
            }
            return new ResetPasswordDTO();
        }
    }
}