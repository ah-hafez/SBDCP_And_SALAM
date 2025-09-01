

using MCS.Common;

namespace MCS.DTO
{
    public class PrivateFollowupDto : BaseFollowupDto
    {
        public PrivateFollowupDto()
        {
            FollowUpTypeId = (int)FollowupType.Privet;
        }
        public int EntityId { get; set; }
        public int? UserId { get; set; }

    }
}
