using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SuggestedTopicViewModel
    {
        public SuggestedTopicVM SuggestedTopic { get; set; }
        public List<SuggestedTopicVM> SuggestedTopics { get; set; }
        public SuggestedTopicViewModel()
        {
            SuggestedTopic = new SuggestedTopicVM();
            SuggestedTopics = new List<SuggestedTopicVM>();
        }
    }
}