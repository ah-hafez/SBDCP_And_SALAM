namespace MCS.DTO
{
    public class DashboardHomeDTO
    {
        public int OutboundCount { get; set; }
        public int OutboundDraftCountCreated { get; set; }
        public int OutboundDraftCountAssigned { get; set; }
        public int InboundCountCreated { get; set; }
        public int InboundCountAssigned { get; set; }
        public int InternalOutboundCountCreated { get; set; }
        public int InternalOutboundCountAssigned { get; set; }
        public int DelayedCount { get; set; }
    }
}
