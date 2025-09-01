using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class ArchivesLibraryDTO
    {
        public string DocumentNum { get; set; }
        public DateTime Date { get; set; }
        public string ConfidentialityLevel { get; set; }
        public string DocumentType { get; set; }
        public string Keywords { get; set; }
        public int PagesNum { get; set; }
        public string Operative { get; set; }
    }
}
