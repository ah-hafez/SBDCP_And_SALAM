using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionLinkDTO
    {
        public int Id { get; set; }
        public int LinkTypeId { get; set; }  //نوع الربط//

        public string LinkTypeName { get; set; }  //نوع الربط//        

        public int TransactionId { get; set; }
        public string TransactionNumber { get; set; }   //رقم المعاملة//

        public int Year { get; set; } //السنة//

        public int OrgUnitId { get; set; }  //الوحدة//

        public string DateH { get; set; }//تاريخ المعاملة//
        public string Date { get; set; }
        public string Subject { get; set; }//الموضوع//
        public string TransactionType { get; set; }//مصدر القيد//
        public int TransactionCategory { get; set; }//نوع المعاملة//
        public string TransactionCategoryName { get; set; }
        public int ConfidentialityId { get; set; }
        public string OrgunitName { get; set; }// الجهة

    }
}