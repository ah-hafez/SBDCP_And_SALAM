using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISuggestedTopicRepository : IRepository<SuggestedTopic>
    {
        int AddSuggestedTopic(SuggestedTopic suggestedTopic);
        void UpdateSuggestedTopic(SuggestedTopic suggestedTopic);
        void DeleteSuggestedTopic(int suggestedTopicId);
        IList<SuggestedTopic> GetAllSuggestedTopics();
        IList<SuggestedTopic> GetSuggestedTopicsByOrgUnitId(int orgUnitId, string cultureName);
    }
}
