using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Models
{
    public class BasicInfoBaseVM
    {
        public DateTime? RemindDate { get; set; }

        public string RemindDateH { get; set; }

        public int? Hour { get; set; }

        public int? Minute { get; set; }

        public List<int> SubjectClassifications { get; set; }

        public int? SuggestedTopicId { get; set; }
    }
}