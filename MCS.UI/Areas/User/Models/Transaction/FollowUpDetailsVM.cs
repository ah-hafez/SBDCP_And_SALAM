using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class FollowUpDetailsVM : EntityBase
    {
        public int Id { get; set; }
        public int TransactionFollowupId { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Note")]
        public string Notes { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.NoteCreatedOn")]
        public string CreatedOn { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Employee")]
        public string UserName { get; set; }

        [CustomDisplayName("User.Transaction.FollowUp.Entity")]
        public string EntityName { get; set; }
    }
}