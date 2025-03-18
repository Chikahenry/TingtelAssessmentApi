using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TingtelAssessment.Application.Interfaces;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;

namespace TingtelAssessment.Application.Commands.CreateCommand
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Entities.Task
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Status = TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                DueDate = request.DueDate
            };

            await _taskRepository.AddAsync(task);
            return task.Id;
        }
    }
}
