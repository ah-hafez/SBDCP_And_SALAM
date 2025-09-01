using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class BasicInfoBaseVM 
    {
        public DateTime? RemindDate { get; set; }

        public string RemindDateH { get; set; }

        public int? Hour { get; set; }

        public int? Minute { get; set; }

        public List<int> SubjectClassifications { get; set; }

        [CustomDisplayName("User.Transaction.BasicInfo.SuggestedTopic")]
        public int? SuggestedTopicId { get; set; }
    }
}