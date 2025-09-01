using MCS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class PublicFollowupDto : BaseFollowupDto
    {
        public PublicFollowupDto()
        {
            FollowUpTypeId = (int)FollowupType.Public;
        }

    }
}
