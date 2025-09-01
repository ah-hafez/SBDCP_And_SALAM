using MCS.Common;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class ExternalPartyViewModel
    {
        public ExternalPartyAddVM AddExternalParty { get; set; }
        public ExternalPartyEditVM EditExternalParty { get; set; }
        public TreeViewModel Tree { get; set; }

        public ExternalPartyViewModel()
        {
            AddExternalParty = new ExternalPartyAddVM();
            EditExternalParty = new ExternalPartyEditVM();
        }
    }
}