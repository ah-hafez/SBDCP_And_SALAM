using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IDocumentAttributeRepository
    {
        List<DocumentAttribute> GetDocumentAttributes();
    }
}