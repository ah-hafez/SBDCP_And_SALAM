using System.Collections.Generic;

namespace MCS.DTO
{
    public class ManagerAddDTO
    {
       public List<LocalizationDTO> Name { get; set; }
       public int PartyId { get; set; }
        public string EmailAddress { get; set; }
    }
}
