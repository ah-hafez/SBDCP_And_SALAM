using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class UpdateUserInformationVM
    {
        [CustomDisplayName("User.UserProfile.PhoneNumber")]
        [CustomRequired("User.UserProfile.PhoneNumber")]
        public string PhoneNumber { get; set; }

        [CustomDisplayName("User.UserProfile.TransferNumber")]
        [CustomRequired("User.UserProfile.TransferNumber")]
        public string TransferNumber { get; set; }
     


    }
}