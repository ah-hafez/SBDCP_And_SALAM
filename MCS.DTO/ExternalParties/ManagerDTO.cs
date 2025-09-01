using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class ManagerDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Name { get; set; }
        public string LocalName { get; set; }
        public DateTime AddedDate { get; set; }
        public int PartyId { get; set; }
        public string EmailAddress { get; set; }
    }
}
