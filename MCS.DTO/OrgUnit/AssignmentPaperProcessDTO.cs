using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AssignmentPaperActionDTO
    {        
        public int Id { get; set; }

        [CustomRequired("Admin.AssignmentPaperActions.ActionRequired")]
        public int ActionId { get; set; }

        public string Name { get; set; }
    }
}
