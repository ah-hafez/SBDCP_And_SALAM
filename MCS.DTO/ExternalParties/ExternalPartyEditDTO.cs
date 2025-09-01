using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class ExternalPartyEditDTO
    {
        public int Id { get; set; }

        public List<LocalizationDTO> Name { get; set; }

        public List<AddressDTO> Address { get; set; }

        //[CustomRequired("Global.ExternalParty.EmailRequired")]
        [CustomEmailAddress("Global.ExternalParty.InvalidEmail")]
        [CustomRegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", "Global.ExternalParty.InvalidEmail")]
        public string Email { get; set; }

        [CustomRequired("Global.ExternalParty.NumberRequired")]
               [CustomStringLength("Global.ExternalParty.PartyNumberLength", 10, 1)]
        public string PartyNumber { get; set; }

        //[CustomRequired("Global.ExternalParty.PhoneRequired")]
               [CustomStringLength("Global.ExternalParty.PhoneNumberLength", 15, 15)]
        public string PhoneNumber { get; set; }

        //[CustomRequired("Global.ExternalParty.FaxRequired")]
               [CustomStringLength("Global.ExternalParty.FaxNumberLength", 10, 10)]
        public string FaxNumber { get; set; }

        public List<ExternalPartyListTypeDTO> Types { get; set; }

        public bool IsVirtual { get; set; }
        public int? ParentId { get; set; }

        public bool IsYesserRegistered { get; set; }
    }
}
