using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class OutboundCertificateDTO
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

        public List<TransactionNameDTO> Names { get; set; }

        public List<TransactionCertificateLinkDTO> Links { get; set; }

        public List<TransactionAttachmentDTO> Attachments { get; set; }

        public List<TransactionCopyDTO> Copies { get; set; }

        public List<TransactionExternalCopyDTO> ExternalCopies { get; set; }

        public DocumentDTO DocumentDTO { get; set; }

        public List<TransactionAssignmentDTO> Assignments { get; set; }

        public List<TransactionCertificateHistoryDTO> TransactionCertificateHistory { get; set; }

        public TransactionAssignmentDTO LatestAssignment { get; set; }

        public bool IsAssignToMoreThanOne { get; set; }

        public string RemindDateH { get; set; }
        public string RemindTime { get; set; }

        public bool HasDate { set; get; }
        public string Remarks { set; get; }
        public string ToEntity { set; get; }
        public int ProcessPeriodTransaction { get; set; }
        public string SideContactExternalEntityName { get; set; }
        public string NumberContact { get; set; }
        public string ClassificationName { get; set; }
        public string FileDescription { get; set; }

        public int FileNumber { get; set; }
    }
}
