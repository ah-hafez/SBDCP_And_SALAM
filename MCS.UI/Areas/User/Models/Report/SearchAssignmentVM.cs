using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Report
{
    public class SearchAssignmentVM
    {
        [CustomDisplayName("User.Transaction.Search.FromOrg")]
        public int FromOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.Search.ToOrg")]
        public int ToOrgUnitId { get; set; }

        [CustomDisplayName("User.Transaction.Search.FromEmployee")]
        public int FromEmployeeId { get; set; }

        [CustomDisplayName("User.Transaction.Search.ToEmployee")]
        public int ToEmployeeId { get; set; }
    }
}