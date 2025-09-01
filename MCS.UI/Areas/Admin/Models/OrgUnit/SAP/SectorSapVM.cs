using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class SectorSapVM
    {
        public D d { get; set; }
    }


    public class D
    {
        public List<SectorResult> results { get; set; }
    }


    public class SectorResult
    {

        public string externalCode { get; set; }
        public string mdfSystemStatus { get; set; }
        public string externalName_ar_SA { get; set; }
        public string externalName_en_US { get; set; }
    }




}