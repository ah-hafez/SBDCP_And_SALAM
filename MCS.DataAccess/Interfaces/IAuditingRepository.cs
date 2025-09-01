using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IAuditingRepository 
    {
        void AddApiLog(ApiAuditLog apiAuditLog);
        int GetLogBySignature(string signature);
    }
}
