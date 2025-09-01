using System;
using System.Collections.Generic;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class MultiExternalOutboundVM : EntityBase
    {
        public int Id { get; set; }
        [CustomDisplayName("User.OutboundExternal.BasicInfo.Destination")]
        public int OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
        public List<MultiExternalOutboundVM> MultipleExternalOutbound { get; set; } = (AjaxGrid<MultiExternalOutboundVM>)new AjaxGridFactory().CreateAjaxGrid(new List<MultiExternalOutboundVM>(), 1, 0, false);

        public string ExternalOrgSelectedList { get; set; }
     


    }
}