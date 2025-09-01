using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.ExternalParties
{
    public class CopiesExternalPartiesVM
    {
        [CustomDisplayName("User.CopiesExternalParties.Destination")]
        [CustomRequired("User.CopiesExternalParties.DestinationRequired")]
        public int DestinationId { get; set; }
    }
}