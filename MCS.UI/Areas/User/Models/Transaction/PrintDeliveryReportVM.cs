using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class PrintDeliveryReportVM
    {
        public int PrintCount{ get; set; }
        public List<DeliveryReportVM> DeliveryReportVM { get; set; }
    }
}