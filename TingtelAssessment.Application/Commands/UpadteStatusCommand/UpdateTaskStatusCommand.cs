using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;

namespace TingtelAssessment.Application.Commands.UpadteStatusCommand
{
    public class UpdateTaskStatusCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public TaskStatus NewStatus { get; set; }
    }
}
