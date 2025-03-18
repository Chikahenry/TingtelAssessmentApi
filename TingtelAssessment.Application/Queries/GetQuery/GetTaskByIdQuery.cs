using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TingtelAssessment.Application.Dtos;

namespace TingtelAssessment.Application.Queries.GetQuery
{
    public class GetTaskByIdQuery : IRequest<TaskDto>
    {
        public Guid Id { get; set; }
    }
}
