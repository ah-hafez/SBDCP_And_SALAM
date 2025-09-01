using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class CounterEditVM
    {
        public int Id { get; set; }
        public bool IsJoinToGeneralCounter { get; set; }
        public List<CounterDetailVM> CounterDetails { get; set; }
        public int Year { get; set; }
    }
}