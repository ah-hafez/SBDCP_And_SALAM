using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models
{
    public class ExternalPartyAttachmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PartyId { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public bool IsDeleted { get; set; }
    }
}