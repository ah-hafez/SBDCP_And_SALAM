using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models
{
    public class UserPendingRequest
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string GroupName { get; set; }
    }
}