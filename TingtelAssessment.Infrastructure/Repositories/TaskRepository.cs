using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;
using TingtelAssessment.Application.Interfaces;
using TingtelAssessment.Infrastructure.Data;

namespace TingtelAssessment.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Task> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks.Where(t => t.Status == status).ToListAsync();
        }

        public async Task AddAsync(Domain.Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Domain.Entities.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
