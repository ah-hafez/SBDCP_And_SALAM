using System;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TaskHistoryRepository : BaseRepository<TaskHistory>, ITaskHistoryRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public TaskHistoryRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public int AddTaskHistory(TaskHistory taskHistory)
        {
            try
            {
                _oMCSDbContext.TaskHistories.Add(taskHistory);

                _oMCSDbContext.SaveChanges();

                return taskHistory.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion
    }
}
