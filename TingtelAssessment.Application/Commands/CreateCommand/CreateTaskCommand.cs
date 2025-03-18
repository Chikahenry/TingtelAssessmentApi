using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TingtelAssessment.Application.Commands.CreateCommand
{
    public class CreateTaskCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
