
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;

namespace TingtelAssessment.Domain.StateMachine
{
    public class TaskStateMachine
    {
        private static readonly Dictionary<TaskStatus, HashSet<TaskStatus>> _allowedTransitions = new Dictionary<TaskStatus, HashSet<TaskStatus>>
        {
            { TaskStatus.Pending, new HashSet<TaskStatus> { TaskStatus.InProgress, TaskStatus.Cancelled } },
            { TaskStatus.InProgress, new HashSet<TaskStatus> { TaskStatus.Completed, TaskStatus.Cancelled } },
            { TaskStatus.Completed, new HashSet<TaskStatus> { TaskStatus.InProgress } },
            { TaskStatus.Cancelled, new HashSet<TaskStatus> { TaskStatus.Pending } }
        };

        public bool CanTransition(TaskStatus currentStatus, TaskStatus newStatus)
        {
            if (!_allowedTransitions.ContainsKey(currentStatus))
                return false;

            return _allowedTransitions[currentStatus].Contains(newStatus);
        }

        public void ValidateTransition(TaskStatus currentStatus, TaskStatus newStatus)
        {
            if (!CanTransition(currentStatus, newStatus))
                throw new InvalidOperationException($"Cannot transition from {currentStatus} to {newStatus}");
        }
    }
}
