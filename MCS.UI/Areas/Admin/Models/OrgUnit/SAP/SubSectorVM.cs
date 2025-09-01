using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class SubSectorVM
    {
        public SubSectorD d { get; set; }
    }

    public class SubSectorD
    {
        public List<SubSectorResult> results { get; set; }
    }
    public class SubSectorResult
    {
        public string externalCode { get; set; }
        public string cust_Name_en_US { get; set; }
        public string cust_Name_ar_SA { get; set; }
        public string mdfSystemStatus { get; set; }
        public ParentSubSector cust_Sector { get; set; }


    }

    public class ParentSubSector 
    {
        public string externalCode { get; set; }

    }

}