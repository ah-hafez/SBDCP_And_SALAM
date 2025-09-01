using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IEscalationRepository : IRepository<Escalation>  
    {
        int AddEscalation(Escalation escalation);
        void UpdateEscalation(Escalation escalation);
        void DeleteEscalation(int id);
        int GetEscalationCategoryId(int EcsalationId);
           int GetEscalationPriorityId(int EcsalationId);
        IList<Escalation> GetEscalations(int TransactionCategoryId,string cultureName);
        IList<Escalation> GetEscalationByPriority(int TransactionCategoryId, int PriorityId, string cultureName);
        Escalation GetEscalationById(int escalationId);
    }
}
