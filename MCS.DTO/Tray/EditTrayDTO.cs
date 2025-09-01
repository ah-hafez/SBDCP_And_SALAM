using System.Collections.Generic;

namespace MCS.DTO
{
    public class EditTrayDTO
    {
       public int Id { get; set; }

       public List<LookupLocalizationDTO>  Names { get; set; }

       public int PermissionId { get; set; }
    }
}
