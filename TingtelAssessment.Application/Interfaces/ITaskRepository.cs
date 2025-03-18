using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;

namespace TingtelAssessment.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<Domain.Entities.Task> GetByIdAsync(Guid id);
        Task<IEnumerable<Domain.Entities.Task>> GetAllAsync();
        Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(TaskStatus status);
        Task AddAsync(Domain.Entities.Task task);
        Task UpdateAsync(Domain.Entities.Task task);
        Task DeleteAsync(Guid id);
    }
}
