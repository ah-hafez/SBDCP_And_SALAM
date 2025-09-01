using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class MoveTransactionVM
    {
        public int TransId { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        [CustomDisplayName("Admin.MoveTransaction.TransactionNumber")]
        [CustomRequired("Admin.MoveTransaction.TransactionNumber.Validation")]
        public int TransNumber { get; set; }
        [CustomDisplayName("Admin.MoveTransaction.TransactionYear")]
        [CustomRequired("Admin.MoveTransaction.TransactionYear.Required")]
        public string TransYear { get; set; }
        [CustomDisplayName("Admin.Actions.MoveEntity.Department")]
        [CustomRequired("Admin.Actions.MoveEntity.DepartmentValiidation")]
        public int EntityFromId { get; set; }
        [CustomDisplayName("Admin.Actions.MoveEntity.Parent")]
        [CustomRequired("Admin.Actions.MoveEntity.ParentValiidation")]
        public int EntityToId { get; set; }
        [CustomDisplayName("Admin.MoveTransaction.FromEmployee")]
        public int DirectedFromId { get; set; }
        [CustomDisplayName("Admin.MoveTransaction.ToEmployee")]
        [CustomRequired("Admin.MoveTransaction.DirectedToId.Required")]
        public int DirectedToId { get; set; }
        [CustomDisplayName("Admin.MoveTransaction.TransactionType")]
        [CustomRequired("Admin.MoveTransaction.TransactionTypeId.Required")]
        public int TransactionTypeId { get; set; }
        public string UsersFromIds { get; set; }
    }
}