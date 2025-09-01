using MCS.Framework.Encryption;
using MCS.DTO;
using MCS.UI.Areas.User.Models.UserProfile;

namespace MCS.UI.Areas.User.Mappers.UserProfile
{
    public static class ChangePasswordMapper
    {
        public static ChangePasswordVM Map(ChangePasswordDTO changePasswordDTO)
        {
            if (changePasswordDTO != null)
            {
                ChangePasswordVM changePasswordVM = new ChangePasswordVM()
                {
                    OldPassword = AESEncrytDecry.EncryptData(changePasswordDTO.OldPassword),
                    NewPassword = AESEncrytDecry.EncryptData(changePasswordDTO.NewPassword),
                    ReNewPassword = AESEncrytDecry.EncryptData(changePasswordDTO.ReNewPassword)
                };

                return changePasswordVM;
            }
            return new ChangePasswordVM();
        }
        public static ChangePasswordDTO Map(ChangePasswordVM changePasswordVM)
        {
            if (changePasswordVM != null)
            {
                ChangePasswordDTO changePasswordDTO = new ChangePasswordDTO()
                {
                    OldPassword = AESEncrytDecry.EncryptData(changePasswordVM.OldPassword),
                    NewPassword = AESEncrytDecry.EncryptData(changePasswordVM.NewPassword),
                    ReNewPassword = AESEncrytDecry.EncryptData(changePasswordVM.ReNewPassword)
                };

                return changePasswordDTO;
            }
            return new ChangePasswordDTO();
        }
    }
}