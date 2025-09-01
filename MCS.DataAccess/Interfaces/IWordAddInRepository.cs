using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IWordAddInRepository : IRepository<WordAddInTemp>
    {

        void SaveTempDocument(byte[] data, string filename);
        WordAddInTemp GetTempDocument(string filename);
        void MarkDocumentAsRead(string filename);

        void UpdateTempDocument(byte[] data, byte[] pdf, string filename);

        WordAddInTemp GetTempDocumentByUserId(int userId);
    }
}
