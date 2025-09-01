using MCS.Common;
using System;
using System.Collections.Generic;

namespace MobileApi.Domain
{
    public class Transaction
    {
        public int TransID { get; set; }
        public string TransNo { get; set; }
        public bool Has_Supporting_Attachments { get; set; }
        public string TransTitle { get; set; }
        public string TransDate { get; set; }
        public string TransFrom { get; set; }
        public int TransCategory { get; set; }
        public string FileSize { get; set; }
        public bool ReadOnly { get; set; }
        public string TransSourceRow { get; set; }
        public string TransNumberRow { get; set; }
        public string EntityName { get; set; }
        public bool IsInternalOutbound { get; set; }
        public DateTime? TransDateGreg { get; set; }
        public bool OutboundDraft { get; set; }
        public string PrivilegeName { get; set; }
        public string Color { get; set; }
        public bool IsDelayed { get; set; } = false;

        public bool IsAppointment { get; set; } = false;
        public int? StatusLevel { get; set; }
        public bool IsCopy { get; set; }
        public List<string> AllowedActions { get; set; }
        public TransactionDateType SourceTray { get; set; }
        public bool IsSigned { get; set; }


    }
}
