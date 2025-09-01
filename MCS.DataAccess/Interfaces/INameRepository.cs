using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface INameRepository : IRepository<Name>
    {
        int AddName(Name name);
        void UpdateName(Name name);
        IList<Name> GetNames(Expression<Func<Name, bool>> @where);
        Name GetNameById(int nameId);
        List<Name> GetCivilIds();
    }
}
