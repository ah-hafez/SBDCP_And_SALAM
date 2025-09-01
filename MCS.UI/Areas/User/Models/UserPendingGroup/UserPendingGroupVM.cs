using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class UserPendingGroupVM
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}