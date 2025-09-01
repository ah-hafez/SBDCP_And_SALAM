using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISignedDeliveryReportRepository : IRepository<SignedDeliveryReport>
    {
        int AddSignedDeliveryReport(SignedDeliveryReport signedDeliveryReport);

        IList<SignedDeliveryReport> GetSignedDeliveryReport(string date ,int? orgunitId);


    }
}
