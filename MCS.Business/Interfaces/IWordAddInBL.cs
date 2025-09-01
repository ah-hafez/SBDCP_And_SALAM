using System.Collections.Generic;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public interface IWordAddInBL
    {
        void SaveTempDocument(byte[] data, string filename);
        WordAddinDocumentDTO GetTempDocument(string filename);
        void MarkDocumentAsRead(string filename);
        void UpdateTempDocument(WordAddinDocumentDTO data, string filename);
        WordAddInTemp GetTempDocumentByUserId(int userId);
    }
}
