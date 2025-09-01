using System;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TaskReminderRepository : BaseRepository<TaskReminder>, ITaskReminderRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public TaskReminderRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public int AddTaskReminder(TaskReminder taskReminder)
        {
            try
            {  
                _oMCSDbContext.TaskReminders.Add(taskReminder);

                _oMCSDbContext.SaveChanges();

                return taskReminder.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
