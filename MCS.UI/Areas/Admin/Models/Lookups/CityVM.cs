using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class CityVM
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public List<LocalizationVM> Description { get; set; }
    }
}