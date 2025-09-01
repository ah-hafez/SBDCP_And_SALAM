using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class SectionSapVM
    {
        public SectionD d { get; set; }
    }


    public class SectionCustToDepartment
    {
        public List<SectionResult> results { get; set; }
    }

    public class SectionD
    {
        public List<SectionResult> results { get; set; }
    }


    public class SectionResult
    {
        public string externalCode { get; set; }
        public string mdfSystemStatus { get; set; }
        public string externalName_ar_SA { get; set; }
        public string externalName_en_US { get; set; }
        public SectionCustToDepartment cust_toDepartment { get; set; }
    }













}