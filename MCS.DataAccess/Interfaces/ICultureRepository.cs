using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ICultureRepository: IRepository<Culture>
    {
        List<Culture> GetCultures();
        Culture GetCultureById(int id);
    }
}
