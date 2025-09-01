using System.Collections.Generic;

namespace MobileApi.Domain
{
    public class UserAuthorization
    {
        public List<Tray> Trays { get; set; }
        public List<TransactionSource> TransactionSources { get; set; }
        public List<Confidentiality> TransactionConfidentialities { get; set; }
        public List<Priority> TransactionPriorities { get; set; }
        public List<TransactionType> TransactionTypes { get; set; }
        public List<AttachmentType> AttachmentTypes { get; set; }
        public List<IncludedItemType> IncludedItemTypes { get; set; }
        public List<Permission> Permissions { get; set; }
        public List<TransactionCategory> TransCategories { get; set; }
        public List<TransactionProcess> Processes { get; set; }
        public List<TransactionPartyDirection> TransactionPartyDirection { get; set; }
        public List<RowStatus> RowStatus { get; set; }
        public List<AttachmentMethod> AttachmentMethods { get; set; }
        public ArchivingType ArchivingTypes { get; set; }
        public List<AttachConfidentiality> AttachConfidentialities { get; set; }
        public TrayID TrayIDs { get; set; }
        public WithAppointmentID WithAppointmentIDs { get; set; }
        public PermissionName PermissionNames { get; set; }
        public List<int> AssignmentPaperProcesses { get; set; }
        public List<UserEntity> Entities { get; set; }


    }

    public class WithAppointmentID
    {
        public int InternalOutbound { get; set; } = 4;
        public int OutboundDraft { get; set; } = 9;
    }

}
