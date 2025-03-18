using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = TingtelAssessment.Domain.Enums.TaskStatus;
using TingtelAssessment.Application.Dtos;


namespace TingtelAssessment.Application.Queries.GetAllQuery
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>
    {
        public TaskStatus? StatusFilter { get; set; }
    }
}
