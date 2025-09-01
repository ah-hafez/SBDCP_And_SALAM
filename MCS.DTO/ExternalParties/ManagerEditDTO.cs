using System.Collections.Generic;

namespace MCS.DTO
{
    public class ManagerEditDTO
    {
       public int Id { get; set; }
       public List<LocalizationDTO> Name { get; set; }
       public int PartyId { get; set; }
        public string EmailAddress { get; set; }
    }
}
