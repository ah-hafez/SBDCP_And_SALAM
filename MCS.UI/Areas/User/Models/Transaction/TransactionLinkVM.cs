using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionLinkVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.Link.LinkType")]

        public int LinkTypeId { get; set; }  //نوع الربط//

        public string LinkTypeName { get; set; }  //نوع الربط//        

        public int TransactionId { get; set; }

        [CustomDisplayName("User.Transaction.Link.TransactionNumber")]
        [CustomRequired("User.Transaction.Link.TransactionNumberRequired")]
        [CustomStringLength("User.Transaction.Link.TransactionNumberLength", 30, 0)]
        public string TransactionNumber { get; set; }   //رقم المعاملة//

        [CustomDisplayName("User.Transaction.Link.Year")]
        [CustomRequired("User.Transaction.Link.YearRequired")]
        public int Year { get; set; } //السنة207//
        public int YearDesc { get; set; } //السنة1441//

        public string DateH { get; set; }//تاريخ المعاملة//
        public string Date { get; set; }

        [CustomDisplayName("User.OutboundExternal.BasicInfo.Subject")]
        [CustomRequired("User.OutboundExternal.BasicInfo.SubjectRequired")]
        [CustomStringLength("User.OutboundExternal.BasicInfo.SubjectLength", 500)]
        public string Subject { get; set; } //الموضوع//

        public string TransactionType { get; set; }//مصدر القيد//

        [CustomDisplayName("User.Transaction.Link.OrgUnit")]
        [CustomRequired("User.Transaction.Link.OrgUnitRequired")]
        public int OrgUnitId { get; set; }  //الوحدة//
        [CustomDisplayName("User.Transaction.Link.TransactionType")]

        public int TransactionCategory { get; set; }
        public string TransactionCategoryName { get; set; }
        public List<TransactionLinkVM> Links { get; set; } = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionLinkVM>(), 1, 0, false);
        public List<TransactionDetailsVM> TransactionLinkSearch { get; set; } = (AjaxGrid<TransactionDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionDetailsVM>(), 1, 0, false);
        public int YearSearch { get; set; }
        public int? ExternalPartyId { get; set; }   //جهة الوارد//

        public int ToTransactionId { get; set; }
        public int TypeId { get; set; }  //نوع الر
        public bool HasPermission { get; set; } = true;
        public bool? WithDocumentNumber { get; set; } = false;
        public string OrgunitName { get; set; }


    }
}