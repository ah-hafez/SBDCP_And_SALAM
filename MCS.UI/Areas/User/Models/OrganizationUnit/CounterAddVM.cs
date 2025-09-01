using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class CounterAddVM
    {
        public int Id { get; set; }
        public bool IsJoinToGeneralCounter { get; set; }
        public List<CounterDetailVM> CounterDetails { get; set; }
    }
}