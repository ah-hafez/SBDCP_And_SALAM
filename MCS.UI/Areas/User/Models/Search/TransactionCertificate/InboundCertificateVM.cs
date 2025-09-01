using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models.Search.TransactionCertificate
{
    public class InboundCertificateVM
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.InboundNumber")]
        public long InboundNumber { get; set; }  //رقم القيد/

        public string HijriDate { get; set; }

        public DateTime Date { get; set; }

        public string OrgUnit { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.InboundDocumentNumber")]
        [CustomRequired("User.Inbound.BasicInfo.InboundDocumentNumberRequired")]
        public string InboundDocumentNumber { get; set; }   //رقم المعاملة الواردة//

        [CustomDisplayName("User.Inbound.BasicInfo.Destination")]
        [CustomRequired("User.Inbound.BasicInfo.DestinationRequired")]
        public string Destination { get; set; }   //جهة الوارد//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public string ConfidentialityLevel { get; set; }    //مستوى السريه//

        public string CreatedByOrgUnit { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.DirectedTo")]
        [CustomRequired("User.Inbound.BasicInfo.DirectedToRequired")]
        public string DirectedTo { get; set; }    //موجهة إلى//

        [CustomDisplayName("User.Inbound.BasicInfo.Source")]
        [CustomRequired("User.Inbound.BasicInfo.SourceRequired")]
        public string TransactionType { get; set; }    //نوع الوارد//

        [CustomDisplayName("User.Inbound.BasicInfo.InboundType")]
        [CustomRequired("User.Inbound.BasicInfo.InboundTypeRequired")]
        public string LetterType { get; set; }   //نوع الخطاب الوارد//


        [CustomDisplayName("User.Inbound.BasicInfo.SignedBy")]
        public string SignedBy { get; set; }  //موقعة من//

        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public string PriorityLevel { get; set; }  //درجة الأسبقية//

        public string RemindDateH { get; set; }
        public string RemindTime { get; set; }

        public string CreatedByUser { get; set; }

        public string Status { get; set; }

        [CustomDisplayName("User.Inbound.BasicInfo.Subject")]
        [CustomRequired("User.Inbound.BasicInfo.SubjectRequired")]
        public string Subject { get; set; }  //الموضوع//

        public List<TransactionNameVM> Names { get; set; }

        public List<TransactionCertificateLinkVM> Links { get; set; }

        public List<TransactionAttachmentVM> Attachments { get; set; }
        public List<ExplanationVM> Explainations { get; set; }
        public DocumentVM DocumentVM { get; set; }

        public List<TransactionAssignmentVM> Assignments { get; set; } = new List<TransactionAssignmentVM>();
        public List<TransactionCopyVM> Copies { get; set; }

        public List<TransactionExternalCopyVM> ExternalCopies { get; set; }

        public List<TransactionCertificateHistoryVM> TransactionCertificateHistory { get; set; }

        public TransactionAssignmentVM LatestAssignment { get; set; }

        public bool IsAssignToMoreThanOne { get; set; }
        public string InboundIntendedPerson { get; set; }
        public bool IsForIndividual { get; set; }
        public string DeliveryMethod { get; set; }
        public bool HasDate { get; set; }
        public string Remarks { get; set; }
        public string ToEntity { get; set; }
        public int ProcessPeriodTransaction { get; set; }
        public string SideContactExternalEntityName { get; set; }
        public string NumberContact { get; set; }
        public int? RecordNumber { get; set; }
        public int ConfidentialityId { get; set; }
        public string LetterNumber { get; set; }
        public bool Encrypted { get; set; }
        public string ClassificationName { get; set; }
        public int FileNumber { get; set; }
        public string FileDescription { get; set; }

    }
}