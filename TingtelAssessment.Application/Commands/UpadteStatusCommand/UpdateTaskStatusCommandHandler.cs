using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TingtelAssessment.Application.Interfaces;
using TingtelAssessment.Domain.StateMachine;

namespace TingtelAssessment.Application.Commands.UpadteStatusCommand
{
    public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly TaskStateMachine _stateMachine;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, TaskStateMachine stateMachine)
        {
            _taskRepository = taskRepository;
            _stateMachine = stateMachine;
        }

        public async Task<bool> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                return false;

            _stateMachine.ValidateTransition(task.Status, request.NewStatus);

            task.Status = request.NewStatus;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);
            return true;
        }
    }

}
