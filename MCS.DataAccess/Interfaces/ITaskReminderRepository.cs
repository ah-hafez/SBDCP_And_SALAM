using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITaskReminderRepository : IRepository<TaskReminder>
    {
        int AddTaskReminder(TaskReminder taskReminder);

    }
}
