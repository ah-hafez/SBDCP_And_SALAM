using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;


namespace MCS.DTO
{
    public class UserPendingGroupDTO
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string GroupName { get; set; }
    }
}
