using System;
using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models.Search.TransactionCertificate
{
    public class OutboundCertificateVM
    {
        public int Id { get; set; }

        public long OutboundNumber { get; set; }

        public string HijriDate { get; set; }

        public DateTime Date { get; set; }

        public string OrgUnit { get; set; }

        public string Destination { get; set; }

        public string ConfidentialityLevel { get; set; }

        public string CreatedByOrgUnit { get; set; }

        public string DirectedTo { get; set; }

        public string TransactionType { get; set; }

        public string SignedBy { get; set; }

        public string PriorityLevel { get; set; }

        public string CreatedByUser { get; set; }

        public string Status { get; set; }

        public string Subject { get; set; }

        public List<TransactionNameVM> Names { get; set; }

        public List<TransactionCertificateLinkVM> Links { get; set; }

        public List<TransactionAttachmentVM> Attachments { get; set; }

        public List<TransactionCopyVM> Copies { get; set; }

        public List<TransactionExternalCopyVM> ExternalCopies { get; set; }

        public DocumentVM DocumentVM { get; set; }

        public List<TransactionAssignmentVM> Assignments { get; set; }

        public List<TransactionCertificateHistoryVM> TransactionCertificateHistory { get; set; }

        public TransactionAssignmentVM LatestAssignment { get; set; }

        public bool IsAssignToMoreThanOne { get; set; }

        public string RemindDateH { get; set; }
        public string RemindTime { get; set; }
        public bool HasDate { set; get; }
        public string Remarks { set; get; }
        public string ToEntity { set; get; }
        public int ProcessPeriodTransaction { get; set; }
        public string ClassificationName { get; set; }
        public string FileDescription { get; set; }

        public int FileNumber { get; set; }

    }
}