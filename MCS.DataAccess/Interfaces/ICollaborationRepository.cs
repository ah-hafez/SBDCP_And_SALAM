using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ICollaborationRepository : IRepository<Collaboration>
    {
        void AddCollaboration(Collaboration collaboration);
        void UpdateCollaboration(Collaboration conversation);
        int GetCollaborationCount(Expression<Func<Collaboration, bool>> where);      
       IList<Collaboration> GetCollaborations(Expression<Func<Collaboration, bool>> where);
       IList<Collaboration> GetCollaborations(Expression<Func<Collaboration, bool>> @where, int pageSize, string cultureName);
       IList<Collaboration> GetCollaborations(Expression<Func<Collaboration, bool>> where, SearchCriteria searchCriteria, string cultureName);
       IList<CollaborationUserInfo> GetAllCollaborationUsers(int userId, string cultureName);
    }
}
