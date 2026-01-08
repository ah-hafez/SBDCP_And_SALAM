using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Entities;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using Action = MCS.Domain.Action;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Web.UI.WebControls.WebParts;
using MCS.Domain.IC;

namespace MCS.DataAccess
{
    public class IC_SUBJECTRepository : BaseRepository<Domain.IC_SUBJECT>, IIC_SUBJECTRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public IC_SUBJECTRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods


        public int AddIC_SUBJECT(IC_SUBJECT icSubject)
        {


            try
            {

                IC_SUBJECT IC_SUBJECTItemCode = _oMCSDbContext.IC_SUBJECTS.Where(x => x.ITEM_CODE == icSubject.ITEM_CODE).FirstOrDefault();


                if (IC_SUBJECTItemCode != null)
                {
                    return -1;
                }

                IC_SUBJECT IC_SUBJECTNumber = _oMCSDbContext.IC_SUBJECTS.Where(x => x.Number == icSubject.Number).FirstOrDefault();


                if (IC_SUBJECTNumber != null)
                {
                    return -2;
                }
                if (icSubject.PARENT_ID != null)
                {
                    IC_SUBJECT parent = _oMCSDbContext.IC_SUBJECTS.Where(x => x.Id == icSubject.PARENT_ID).FirstOrDefault();
                    if (parent != null)
                    {
                        icSubject.FULL_CODE = parent.FULL_CODE + "/" + icSubject.ITEM_CODE;

                    }
                }
                else
                {
                    icSubject.FULL_CODE = icSubject.ITEM_CODE;
                }
                var firstIndex = _oMCSDbContext.IC_INDEX.FirstOrDefault();
                icSubject.CONFID_ID = 1;
                icSubject.IS_USED = true;
                icSubject.IcIndexId = firstIndex.Id;
                //icSubject.ClassificationId = icSubject.ClassificationId;
                _oMCSDbContext.IC_SUBJECTS.Add(icSubject);

                _oMCSDbContext.SaveChanges();

                return icSubject.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }


        }

        public void DeleteIC_SUBJECT(int id)
        {
            try
            {


                var result =

           _oMCSDbContext.Database.SqlQuery<int>("DeleteIC @IcId",
                                    new SqlParameter("IcId", id)
                                    ).FirstOrDefault();



                if (result == 0)
                {
                    throw new Exception();
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public IList<IC_SUBJECT> GetIC_SUBJECS(SearchCriteria searchCriteria, out int rowsCount, string cultureName)
        {
            throw new NotImplementedException();
        }

        public IList<IC_SUBJECT> GetAllIC_SUBJECS(string cultureName)
        {
            throw new NotImplementedException();
        }


        public int UpdateIC_SUBJECT(IC_SUBJECT icSubject)
        {
            try
            {


                IC_SUBJECT IC_SUBJECTItemCode = _oMCSDbContext.IC_SUBJECTS.Where(x => x.ITEM_CODE == icSubject.ITEM_CODE && x.Id != icSubject.Id).FirstOrDefault();


                if (IC_SUBJECTItemCode != null)
                {
                    return -1;
                }

                IC_SUBJECT IC_SUBJECTNumber = _oMCSDbContext.IC_SUBJECTS.Where(x => x.Number == icSubject.Number && x.Id != icSubject.Id).FirstOrDefault();


                if (IC_SUBJECTNumber != null)
                {
                    return -2;
                }


                IC_SUBJECT icSubjectOld = GetIC_SUBJECTById(icSubject.Id);

                if (icSubjectOld != null)
                {
                    icSubjectOld.ITEM_CODE = icSubject.ITEM_CODE;
                    icSubjectOld.ITEM_DESCRIPTION_AR = icSubject.ITEM_DESCRIPTION_AR;
                    icSubjectOld.ITEM_DISPLAY = icSubject.ITEM_DISPLAY;
                    //icSubjectOld.PARENT_ID = icSubject.PARENT_ID;
                    icSubjectOld.ACTIVE = icSubject.ACTIVE;
                    _oMCSDbContext.Entry(icSubjectOld).State = EntityState.Modified;
                    _oMCSDbContext.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<IC_SUBJECT> GetIC_SUBJECTByParentId(int? Id, string name)
        {
            try
            {

                var icSubject = _oMCSDbContext.IC_SUBJECTS.AsQueryable();
                if (Id.HasValue && Id.Value > 0)
                {
                    icSubject = icSubject.Where(x => x.PARENT_ID == Id);
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    icSubject = icSubject.Where(x => x.ITEM_DISPLAY.Contains(name) || x.ITEM_DESCRIPTION_AR.Contains(name) || x.ITEM_CODE.Contains(name));
                }
                else
                {
                    icSubject = icSubject.Where(x => x.PARENT_ID == Id);
                }
                var icSubjects = icSubject.Distinct().ToList();
                foreach (var item in icSubjects)
                {

                    item.HasChilds = IsdIC_SUBJECTHasLeaf(item.Id);

                }

                return icSubjects;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public IC_SUBJECT GetIC_SUBJECTById(int id)
        {
            try
            {
                return this.FindBy(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public List<IC_CLASSIFICATION> GetClassificationTypes()
        {
            try
            {
                return _oMCSDbContext.IC_CLASSIFICATIONS.ToList();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public bool IsdIC_SUBJECTHasLeaf(int id)
        {
            try
            {

                bool result = false;
                int count = 0;
                count = _oMCSDbContext.IC_SUBJECTS.Where(x => x.PARENT_ID == id).Count();

                if (count > 0)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public int AddIC_SUBJECT_TRANSACTION(int transId, int ic_id, int? number, string description, int createdBy)
        {
            try
            {
                IC_SUBJECTS_TRANSACTION icTrns = new IC_SUBJECTS_TRANSACTION();

                icTrns.TransactionId = transId;
                icTrns.IC_SUBJECTId = ic_id;
                icTrns.Number = number;
                icTrns.Description = description;
                icTrns.CreatedBy = createdBy;
                IC_SUBJECTS_TRANSACTION icTrnsOld = _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Where(x => x.TransactionId == transId).FirstOrDefault();

                if (icTrnsOld != null)
                {
                    _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Remove(icTrnsOld);
                    _oMCSDbContext.SaveChanges();
                }



                _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Add(icTrns);

                _oMCSDbContext.SaveChanges();

                return icTrns.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public MCS.Domain.Transaction GetTransactionById(int transactionId)
        {
            try
            {
                return _oMCSDbContext.Transactions.Where(t => t.Id == transactionId && !t.IsDeleted).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void RemoveIC_SUBJECT_TRANSACTION(int transId, int ic_id)
        {
            try
            {
                IC_SUBJECTS_TRANSACTION icTrnsOld = _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Where(x => x.TransactionId == transId).FirstOrDefault();

                _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Remove(icTrnsOld);
                _oMCSDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public IC_SUBJECTS_TRANSACTION IC_GetTransaction(int transId)
        {
            try
            {
                IC_SUBJECTS_TRANSACTION icTrnsOld = _oMCSDbContext.IC_SUBJECTS_TRANSACTIONS.Where(x => x.TransactionId == transId).FirstOrDefault();

                return icTrnsOld;

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

       

        #endregion
    }
}
