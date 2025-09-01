using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;
using System;

namespace MCS.Business
{
    public interface ISignedDeliveryReportBL
    {
        IList<SignedDeliveryReport> GetSignedDeliveryReport(string date, int? orgunitId);
    }
}
