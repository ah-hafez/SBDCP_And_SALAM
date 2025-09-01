namespace MCS.Domain
{
    public class PerformanceMeasurementReportResult
    {
        public int OrgUnitsID { get; set; }
        public string OrgUnitName { get; set; }

        public int? UserProfilesID { get; set; }
        public string UserProfileName { get; set; }

        public int OutboundCount { get; set; }

        public int OutboundDraftCountCreated { get; set; }
        public int OutboundDraftCountAssigned { get; set; }

        public int InboundCountCreated { get; set; }
        public int InboundCountAssigned { get; set; }

        public int InternalOutboundCountCreated { get; set; }
        public int InternalOutboundCountAssigned { get; set; }

        public int DelayedCount { get; set; }

        public int FinishedCount { get; set; }
        public int SavedCount { get; set; }
        public int InProgressCount { get; set; }
        public int AssignedCount { get; set; }
    }
}
