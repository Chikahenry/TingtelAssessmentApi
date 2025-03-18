using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TingtelAssessment.Application.Dtos;
using TingtelAssessment.Application.Interfaces;

namespace TingtelAssessment.Application.Queries.GetQuery
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<TaskDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"task:{request.Id}";
            var cachedTask = await _cacheService.GetAsync<TaskDto>(cacheKey);

            if (cachedTask != null)
                return cachedTask;

            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                return null;

            var taskDto = MapToDto(task);
            await _cacheService.SetAsync(cacheKey, taskDto, TimeSpan.FromMinutes(10));

            return taskDto;
        }

        private TaskDto MapToDto(Domain.Entities.Task task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                DueDate = task.DueDate
            };
        }
    }
}
