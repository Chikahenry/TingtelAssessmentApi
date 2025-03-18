using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TingtelAssessment.Application.Commands.CreateCommand;
using TingtelAssessment.Application.Commands.DeleteCommand;
using TingtelAssessment.Application.Commands.UpadteStatusCommand;
using TingtelAssessment.Application.Commands.UpdateCommand;
using TingtelAssessment.Application.Dtos;
using TingtelAssessment.Application.Queries.GetAllQuery;
using TingtelAssessment.Application.Queries.GetQuery;

namespace TingtelAssessment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<TasksController> _logger;

        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all tasks with optional status filter
        /// </summary>
        /// <param name="status">Optional status filter</param>
        /// <returns>List of tasks</returns>
        /// <response code="200">Returns the list of tasks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks([FromQuery] Domain.Enums.TaskStatus? status = null)
        {
            _logger.LogInformation("Getting all tasks with status filter: {Status}", status);

            var query = new GetAllTasksQuery { StatusFilter = status };
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Get a task by its ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>Task details</returns>
        /// <response code="200">Returns the task</response>
        /// <response code="404">If task is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskDto>> GetTaskById(Guid id)
        {
            _logger.LogInformation("Getting task with ID: {TaskId}", id);

            var query = new GetTaskByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="command">Task details</param>
        /// <returns>Created task ID</returns>
        /// <response code="201">Returns the ID of the created task</response>
        /// <response code="400">If the task data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateTask([FromBody] CreateTaskCommand command)
        {
            _logger.LogInformation("Creating new task: {TaskTitle}", command.Title);

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTaskById), new { id = result }, result);
        }

        /// <summary>
        /// Update an existing task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="command">Updated task details</param>
        /// <returns>No content</returns>
        /// <response code="204">If the task was updated successfully</response>
        /// <response code="404">If the task is not found</response>
        /// <response code="400">If the task data is invalid</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskCommand command)
        {
            _logger.LogInformation("Updating task with ID: {TaskId}", id);

            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Update a task's status
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="command">Status update details</param>
        /// <returns>No content</returns>
        /// <response code="204">If the status was updated successfully</response>
        /// <response code="404">If the task is not found</response>
        /// <response code="400">If the status transition is invalid</response>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusCommand command)
        {
            _logger.LogInformation("Updating status for task with ID: {TaskId} to {NewStatus}", id, command.NewStatus);

            if (id != command.Id)
                return BadRequest("ID mismatch");

            try
            {
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid status transition for task {TaskId}", id);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns>No content</returns>
        /// <response code="204">If the task was deleted successfully</response>
        /// <response code="404">If the task is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            _logger.LogInformation("Deleting task with ID: {TaskId}", id);

            var command = new DeleteTaskCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
