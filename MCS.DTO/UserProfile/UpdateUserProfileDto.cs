using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class UpdateUserProfileDto
    {
        public string PhoneNumber { get; set; }
        public string InternalNumber { get; set; }
        public int UserProfileId { get; set; }

    }
}
