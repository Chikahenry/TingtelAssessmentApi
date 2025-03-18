using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TingtelAssessment.Application.Dtos;
using TingtelAssessment.Application.Interfaces;

namespace TingtelAssessment.Application.Queries.GetAllQuery
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = request.StatusFilter.HasValue
                ? $"tasks:status:{request.StatusFilter}"
                : "tasks:all";

            var cachedTasks = await _cacheService.GetAsync<IEnumerable<TaskDto>>(cacheKey);

            if (cachedTasks != null)
                return cachedTasks;

            var tasks = request.StatusFilter.HasValue
                ? await _taskRepository.GetByStatusAsync(request.StatusFilter.Value)
                : await _taskRepository.GetAllAsync();

            var taskDtos = MapToDtoList(tasks);
            await _cacheService.SetAsync(cacheKey, taskDtos, TimeSpan.FromMinutes(5));

            return taskDtos;
        }

        private IEnumerable<TaskDto> MapToDtoList(IEnumerable<Domain.Entities.Task> tasks)
        {
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    CreatedAt = task.CreatedAt,
                    UpdatedAt = task.UpdatedAt,
                    DueDate = task.DueDate
                });
            }
            return taskDtos;
        }
    }

}
