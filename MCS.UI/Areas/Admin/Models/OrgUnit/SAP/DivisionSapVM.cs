using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class DivisionSapVM
    {
        public DivisionSapD d { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CustSector
    {
        public List<DivisionResult> results { get; set; }
    }
    public class CustSubSector
    {
        public List<SubSectorResult> results { get; set; }
    }
    public class DivisionSapD
    {
        public List<DivisionResult> results { get; set; }
    }

    public class DivisionResult
    {
        public string externalCode { get; set; }
        public string name_en_US { get; set; }
        public string name_ar_SA { get; set; }
        public string status { get; set; }
        public CustSector cust_Sector { get; set; }
        public CustSubSector cust_Sub_Sector { get; set; }

    }








}