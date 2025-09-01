using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ISuggestedTopicBL
    {
       IList<SuggestedTopic> GetAllSuggestedTopics();
       void SaveSuggestedTopics(IList<SuggestedTopic> suggestedTopics, out IList<int> suggestedTopicsUsed);
       IList<SuggestedTopic> GetSuggestedTopicsByOrgUnitId(int OrgUnitId, string cultureName);
    }
}
