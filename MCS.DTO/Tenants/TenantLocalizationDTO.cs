using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Tenants
{
    public class TenantLocalizationDTO: BaseDTO
    {
        public int CultureId { get; set; }
        public TenantCultureDTO Culture { get; set; }
        public string Text { get; set; }
        public TenantLocalizationIdentifierDTO LocalizationIdentifier { get; set; }
    }
}
