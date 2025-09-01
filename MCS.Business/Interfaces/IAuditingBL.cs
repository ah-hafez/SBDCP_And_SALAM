using MCS.Domain;

namespace MCS.Business
{
    public interface IAuditingBL
    {
        void AddApiAuditLog(ApiAuditLog apiAuditLog);
        int GetLogBySignature(string signature);
    }
}
