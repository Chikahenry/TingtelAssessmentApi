using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TingtelAssessment.Application.Commands.DeleteCommand
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
