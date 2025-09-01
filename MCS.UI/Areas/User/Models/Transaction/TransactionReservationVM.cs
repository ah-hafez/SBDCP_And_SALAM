using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionReservationVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.Reservation.User")]
        [CustomRequired("User.Transaction.Reservation.UserRequired")]
        public int UserId { get; set; }

        [CustomDisplayName("User.Transaction.Reservation.OrgUnit")]
        [CustomRequired("User.Transaction.Reservation.OrgUnitRequired")]
        public int? EntityId { get; set; }

        [CustomDisplayName("User.Transaction.Reservation.Count")]
        [CustomStringLength("User.Transaction.Reservation.CountLength", 3)]
        [CustomRequired("User.Transaction.Reservation.CountLength")]
        [CustomNumberCompareAttribute("Value", Operation.GreaterThan, "العدد يجب ان يكون اكبر من صفر")]
        public int Count { get; set; }

        public int Value { get; set; } = 0;

        [CustomDisplayName("User.Transaction.Reservation.Reason")]
        [CustomRequired("User.Transaction.Reservation.ReasonRequired")]
        [CustomStringLength("User.Transaction.Reservation.ReasonLength", 500)]
        public string Reason { get; set; }

        [CustomDisplayName("User.Transaction.Reservation.Type")]
        [CustomRequired("User.Transaction.Reservation.TypeRequired")]
        public int TransactionCategoryId { get; set; }

        public string DateTimeHJ { get; set; }
        public string EntityName { get; set; }
        public string UserName { get; set; }
        public string TransactionCategoryName { get; set; }

        public AjaxGrid<TransactionReservationVM> Reservations { get; set; } = (AjaxGrid<TransactionReservationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionReservationVM>(), 1, 0, false);
    }

    public class TransactionReservedVM
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }

        public AjaxGrid<TransactionReservedVM> Transactions { get; set; } = (AjaxGrid<TransactionReservedVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionReservedVM>(), 1, 0, false);

    }
}