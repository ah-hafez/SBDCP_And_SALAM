using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class BasicInfoBaseDTO
    {
        public DateTime? RemindDate { get; set; }

        public string RemindDateH { get; set; }

        public int? Hour { get; set; }

        public int? Minute { get; set; }

        //[CustomDisplayName("User.Transaction.BasicInfo.SubjectClassifications")]
        public List<int> SubjectClassifications { get; set; }

        //[CustomDisplayName("User.Transaction.BasicInfo.SuggestedTopic")]
        public int? SuggestedTopicId { get; set; }

    }
}
