using System;
namespace MCS.UI.Areas.User.Models.Shared
{
    public class RulesAndRegulations
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Amendment { get; set; }

    }
}