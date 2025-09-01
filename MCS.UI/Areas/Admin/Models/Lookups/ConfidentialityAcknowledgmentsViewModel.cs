namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class ConfidentialityAcknowledgmentsViewModel
    {
        public ConfidentialityAcknowledgmentsVM ConfidentialityAcknowledgments { get; set; }
        public ConfidentialityAcknowledgmentsAddVM AddConfidentialityAcknowledgments { get; set; }
        public ConfidentialityAcknowledgmentsEditVM EditConfidentialityAcknowledgments { get; set; }

        public ConfidentialityAcknowledgmentsViewModel()
        {
            ConfidentialityAcknowledgments = new ConfidentialityAcknowledgmentsVM();
            AddConfidentialityAcknowledgments = new ConfidentialityAcknowledgmentsAddVM();
            EditConfidentialityAcknowledgments = new ConfidentialityAcknowledgmentsEditVM();
        }
    }
}