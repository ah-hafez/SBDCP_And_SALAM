using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface IEscalationBL
    {
        int AddEscalation(Escalation escalation);
        void UpdateEscalation(Escalation escalation);
        void DeleteEscalation(int id);
        IList<Escalation> GetEscalations(int TransactionCategoryId, string cultureName);
        IList<Escalation> GetEscalationByPriority(int TransactionCategoryId, int PriorityId, string cultureName);
        Escalation GetEscalationById(int EscalationId);
        int GetEscalationCategoryId(int EscalationId);
        int GetEscalationPriorityId(int EscalationId);
    }
}
