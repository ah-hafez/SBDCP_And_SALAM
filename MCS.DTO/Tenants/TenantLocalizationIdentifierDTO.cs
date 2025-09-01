using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantLocalizationIdentifierDTO: BaseDTO
    {
        public List<TenantLocalizationDTO> Localizations { get; set; }
    }
}
