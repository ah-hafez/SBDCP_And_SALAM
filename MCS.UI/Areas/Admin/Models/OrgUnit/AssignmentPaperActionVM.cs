using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class AssignmentPaperActionVM
    {
        public int Id { get; set; }

        [CustomRequired("User.Transaction.Assignment.ActionIdRequired")]
        public int ActionId { get; set; }

        public string Name { get; set; }
    }
}