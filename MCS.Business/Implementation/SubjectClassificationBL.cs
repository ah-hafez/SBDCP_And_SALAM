using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class SubjectClassificationBL : BaseBL, ISubjectClassificationBL
    {
        public IList<SubjectClassification> GetAllSubjectClassifications()
        {
            try
            {
                ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();
                return subjectClassificationRepository.GetAllSubjectClassifications();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void SaveSubjectClassifications(IList<SubjectClassification> subjectClassifications, out IList<int> subjectClassificationsUsed)
        {
            try
            {
                IList<int> itemsUsed = new List<int>();
                subjectClassifications.ToList().ForEach(s =>
                {
                    if (s.IsDeleted)
                    {
                        IList<Transaction> transactions = TransactionBL.GetTransactions(t =>
                            t.SubjectClassifications.Any(sc => sc.SubjectClassificationId == s.Id));

                        if (transactions.Count > 0)
                            itemsUsed.Add(s.Id);
                    }
                });

                subjectClassificationsUsed = itemsUsed;

                if (itemsUsed.Count != 0)
                    throw new BusinessException(StatusCode.SubjectClassificationRelatedToTransactions);

                ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();

                DeleteByParentId(subjectClassifications.Where(s => s.IsDeleted).ToList());

                AddByParentId(subjectClassifications.Where(s => s.IsNew).ToList());

                subjectClassifications.Where(s => !s.IsNew && !s.IsDeleted).ToList().ForEach(sc =>
                {
                    subjectClassificationRepository.UpdateSubjectClassification(sc);
                });
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<SubjectClassification> GetSubjectClassificationByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();
                return subjectClassificationRepository.GetSubjectClassificationByOrgUnitId(OrgUnitId, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private void DeleteByParentId(IList<SubjectClassification> deletedSubjectClassifications)
        {
            ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();
            deletedSubjectClassifications.Where(s => deletedSubjectClassifications.All(sc => sc.ParentId != s.Id)).ToList().ForEach(s =>
            {
                subjectClassificationRepository.DeleteSubjectClassification(s.Id);
                deletedSubjectClassifications.Remove(s);
            });

            if (deletedSubjectClassifications.Count > 0)
            {
                DeleteByParentId(deletedSubjectClassifications);
            }
        }

        private void AddByParentId(IList<SubjectClassification> newSubjectClassifications)
        {
            ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();

            newSubjectClassifications.Where(s => newSubjectClassifications.All(sc => sc.Id != s.ParentId)).ToList().ForEach(s =>
            {
                subjectClassificationRepository.AddSubjectClassification(s);
                newSubjectClassifications.Remove(s);
            });

            if (newSubjectClassifications.Count > 0)
            {
                AddByParentId(newSubjectClassifications);
            }
        }
        public SubjectClassification GetSubjectClassificationById(int subjectClassificationId)
        {
            try
            {
                ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();
                return subjectClassificationRepository.GetSubjectClassificationById(subjectClassificationId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateSubjectClassification(SubjectClassification subjectClassification)
        {
            try
            {
                ISubjectClassificationRepository subjectClassificationRepository = IoC.Resolve<SubjectClassificationRepository>();
                subjectClassificationRepository.UpdateSubjectClassification(subjectClassification);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
