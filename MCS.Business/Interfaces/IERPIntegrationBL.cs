using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;

namespace MCS.Business
{
    public interface IERPIntegrationBL
    {
        void AddUserSync();
        void DeleteUserSync();
        void MoveUserSync();
        void DelegationUserSync();
        void AddEntitySync();
        void MoveEntitySync();
        void UpdateEntityNameSync();
    }
}
