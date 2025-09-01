using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IActionRepository : IRepository<Action>
    {
        int AddAction(Domain.Action action);
        void DeleteAction(int id);
        void UpdateAction(Domain.Action action);
        IList<Action> GetActions(SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        IList<Action> GetAllActions(string cultureName);
        bool CheckIfActionUsed(int processId);
        void LockUnlockLookup(int ActionId, int UserId);
        void ActiveDeactiveLookup(int ActionId);
        void ChangeEntitiesNameBeforeMove(ChangeEntityName changeEntityName);
        List<UsersClearance> CheckUserClearance(List<int> usersIds, string cultureName);
    }
}
