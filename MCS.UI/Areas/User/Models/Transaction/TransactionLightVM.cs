using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionLightVM
    {
        public int Id { get; set; }
        public TransactionCategory TransactionCategory { get; set; }

        [CustomDisplayName("User.Transaction.Number")]
        [CustomStringLength("Global.Localization.Text", 50, 0)]
        public string Number { get; set; }
        [CustomDisplayName("User.Transaction.Barcode")]
        public string Barcode { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
    }
}