using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using System.Data.Entity;

namespace MCS.DataAccess
{
    public class SignedDeliveryReportRepository : BaseRepository<SignedDeliveryReport>, ISignedDeliveryReportRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SignedDeliveryReportRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Method

        public int AddSignedDeliveryReport(SignedDeliveryReport signedDeliveryReport)
        {
            try
            {
                _oMCSDbContext.SignedDeliveryReports.Add(signedDeliveryReport);
                _oMCSDbContext.SaveChanges();
                return signedDeliveryReport.DocumentId.Value;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<SignedDeliveryReport> GetSignedDeliveryReport(string date, int? orgunitId )
        {
            try
            {
                bool getAll = string.IsNullOrWhiteSpace(date);
                List<SignedDeliveryReport> signedDeliveryReports = _oMCSDbContext.SignedDeliveryReports.Include(x => x.Document).Include(x => x.Document.Document).Where(d => (getAll || d.DateH == date) && (!orgunitId.HasValue || d.TransactionDeliveryReport.OrgunitId == orgunitId)).ToList();
                return signedDeliveryReports;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Method
    }
}
