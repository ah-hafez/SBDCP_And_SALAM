using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantLookupDTO: BaseDTO
    {
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
        public int? EnumReference { get; set; }
        public List<TenantLookupLocalizationDTO> Localizations { get; set; }
        public string Text { get; set; }
    }
}
