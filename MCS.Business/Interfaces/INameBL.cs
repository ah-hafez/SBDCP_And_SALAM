using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface INameBL
    {
        int AddName(Name name);
        void UpdateName(Name name);
        Name GetNameByCivilId(string civilID);
        List<Name> GetCivilIds();
    }
}
