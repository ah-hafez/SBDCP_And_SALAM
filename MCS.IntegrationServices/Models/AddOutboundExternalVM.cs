using MCS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class AddOutboundExternalVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.ExternalOutbound; }
        }
        public AddOutboundExternalBasicInfoVM OutboundExternalBasicInfo { get; set; } = new AddOutboundExternalBasicInfoVM();
        public int? EditorTypeId { get; set; }

    }
}