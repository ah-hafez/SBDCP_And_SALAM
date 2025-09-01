using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class DepartmentSapVM
    {
        public DepartmentSapD d { get; set; }

    }


    public class DepartmentSapCustToDivision
    {
        public List<DepartmentSapResult> results { get; set; }
    }

    public class DepartmentSapD
    {
        public List<DepartmentSapResult> results { get; set; }
    }

    public class DepartmentSapResult
    {
        public string externalCode { get; set; }
        public string name_en_US { get; set; }
        public string name_ar_SA { get; set; }
        public string status { get; set; }
        public DepartmentSapCustToDivision cust_toDivision { get; set; }
    }








}