using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITaskHistoryRepository: IRepository<TaskHistory>
    {
        int AddTaskHistory(TaskHistory taskHistory);
    }
}
