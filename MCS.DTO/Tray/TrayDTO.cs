using System.Collections.Generic;

namespace MCS.DTO
{
    public class TrayDTO
    {
        public int Id { get; set; }

        public IList<LookupLocalizationDTO> Names { get; set; }

        public string LocalName { get; set; }

        public string Permission { get; set; }

        public bool IsSelected { get; set; }

        public int Sort { get; set; }
    }
}
