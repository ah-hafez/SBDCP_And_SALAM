using System;
using System.Collections.Generic;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class MultiInternalOutboundVM : EntityBase
    {
        public int Id { get; set; }
        [CustomDisplayName("User.Inbound.BasicInfo.DirectedToOrgUnit")] 
        public int OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
        public List<MultiInternalOutboundVM> MultipleInternalOutbound { get; set; } = (AjaxGrid<MultiInternalOutboundVM>)new AjaxGridFactory().CreateAjaxGrid(new List<MultiInternalOutboundVM>(), 1, 0, false);

        public string InternalOrgSelectedList { get; set; }
     


    }
}