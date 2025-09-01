using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;


namespace MCS.Business
{
    public class SuggestedTopicBL : BaseBL, ISuggestedTopicBL
    {
        public IList<SuggestedTopic> GetAllSuggestedTopics()
        {
            try
            {
                ISuggestedTopicRepository suggestedTopicRepository = IoC.Resolve<SuggestedTopicRepository>();
                return suggestedTopicRepository.GetAllSuggestedTopics();
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

        public IList<SuggestedTopic> GetSuggestedTopicsByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                ISuggestedTopicRepository suggestedTopicRepository = IoC.Resolve<SuggestedTopicRepository>();
                return suggestedTopicRepository.GetSuggestedTopicsByOrgUnitId(OrgUnitId, cultureName);
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

        public void SaveSuggestedTopics(IList<SuggestedTopic> suggestedTopics, out IList<int> subjectClassificationsUsed)
        {
            try
            {
                IList<int> itemsUsed = new List<int>();

                suggestedTopics.ToList().ForEach(s =>
                {
                    if (s.IsDeleted)
                    {
                        IList<Transaction> transactions = TransactionBL.GetTransactions(t =>
                            t.SuggestedTopicId == s.Id);

                        if (transactions.Count > 0)
                            itemsUsed.Add(s.Id);
                    }
                });

                subjectClassificationsUsed = itemsUsed;

                if (itemsUsed.Count != 0)
                    throw new BusinessException(StatusCode.SuggestedTopicRelatedToTransactions);


                ISuggestedTopicRepository suggestedTopicRepository = IoC.Resolve<SuggestedTopicRepository>();

                DeleteByParentId(suggestedTopics.Where(s => s.IsDeleted).ToList());

                AddByParentId(suggestedTopics.Where(s => s.IsNew).ToList());

                suggestedTopics.Where(s => !s.IsNew && !s.IsDeleted).ToList().ForEach(sc =>
                {
                    suggestedTopicRepository.UpdateSuggestedTopic(sc);
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

        private void DeleteByParentId(IList<SuggestedTopic> deletedSuggestedTopics)
        {
            ISuggestedTopicRepository suggestedTopicRepository = IoC.Resolve<SuggestedTopicRepository>();

            deletedSuggestedTopics.Where(s => deletedSuggestedTopics.All(sc => sc.ParentId != s.Id)).ToList().ForEach(s =>
            {
                suggestedTopicRepository.DeleteSuggestedTopic(s.Id);
                deletedSuggestedTopics.Remove(s);
            });

            if (deletedSuggestedTopics.Count > 0)
            {
                DeleteByParentId(deletedSuggestedTopics);
            }
        }

        private void AddByParentId(IList<SuggestedTopic> newSuggestedTopics)
        {
            ISuggestedTopicRepository suggestedTopicRepository = IoC.Resolve<SuggestedTopicRepository>();

            newSuggestedTopics.Where(s => newSuggestedTopics.All(sc => sc.Id != s.ParentId)).ToList().ForEach(s =>
            {
                suggestedTopicRepository.AddSuggestedTopic(s);
                newSuggestedTopics.Remove(s);
            });

            if (newSuggestedTopics.Count > 0)
            {
                AddByParentId(newSuggestedTopics);
            }
        }
    }
}
