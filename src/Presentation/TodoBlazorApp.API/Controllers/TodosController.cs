using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Application.Features.Todos.Commands;
using TodoBlazorApp.Application.Features.Todos.Queries;
using TodoBlazorApp.Infrastructure.Services;

namespace TodoBlazorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TodosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TodosController> _logger;

    public TodosController(
        IMediator mediator,
        ICurrentUserService currentUserService,
        ILogger<TodosController> logger)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    /// <summary>
    /// الحصول على جميع المهام
    /// </summary>
    /// <param name="includeDetails">تضمين التفاصيل (التعليقات والمرفقات)</param>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAll([FromQuery] bool includeDetails = false)
    {
        _logger.LogInformation("GetAll todos requested by {User}, IncludeDetails: {IncludeDetails}",
            _currentUserService.UserName, includeDetails);

        var result = await _mediator.Send(new GetAllTodosQuery(includeDetails));
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// الحصول على مهمة محددة
    /// </summary>
    /// <param name="id">معرف المهمة</param>
    /// <param name="includeDetails">تضمين التفاصيل</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItemDto>> GetById(int id, [FromQuery] bool includeDetails = true)
    {
        _logger.LogInformation("GetById {TodoId} requested by {User}", id, _currentUserService.UserName);

        var result = await _mediator.Send(new GetTodoByIdQuery(id, includeDetails));
        
        if (!result.IsSuccess)
        {
            return NotFound(new { message = result.Errors.FirstOrDefault() });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// الحصول على إحصائيات المهام
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(TodoStatsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TodoStatsDto>> GetStats()
    {
        _logger.LogInformation("GetStats requested by {User}", _currentUserService.UserName);

        var result = await _mediator.Send(new GetTodoStatsQuery());
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// الحصول على المهام المتأخرة
    /// </summary>
    [HttpGet("overdue")]
    [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetOverdue()
    {
        _logger.LogInformation("GetOverdue todos requested by {User}", _currentUserService.UserName);

        var result = await _mediator.Send(new GetOverdueTodosQuery());
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// إنشاء مهمة جديدة
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoItemDto>> Create([FromBody] CreateTodoItemDto createDto)
    {
        _logger.LogInformation("Create todo requested by {User}: {Title}",
            _currentUserService.UserName, createDto.Title);

        var result = await _mediator.Send(
            new CreateTodoCommand(createDto, _currentUserService.UserId ?? "hnjm"));
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// تحديث مهمة موجودة
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItemDto>> Update(int id, [FromBody] UpdateTodoItemDto updateDto)
    {
        if (id != updateDto.Id)
        {
            return BadRequest(new { message = "معرف المهمة غير متطابق" });
        }

        _logger.LogInformation("Update todo {TodoId} requested by {User}",
            id, _currentUserService.UserName);

        var result = await _mediator.Send(
            new UpdateTodoCommand(updateDto, _currentUserService.UserId ?? "hnjm"));
        
        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// حذف مهمة
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Delete todo {TodoId} requested by {User}",
            id, _currentUserService.UserName);

        var result = await _mediator.Send(new DeleteTodoCommand(id));
        
        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors });
        }

        return NoContent();
    }

    /// <summary>
    /// تبديل حالة الإكمال للمهمة
    /// </summary>
    [HttpPatch("{id:int}/toggle")]
    [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItemDto>> ToggleComplete(int id)
    {
        _logger.LogInformation("Toggle complete for todo {TodoId} by {User}",
            id, _currentUserService.UserName);

        var result = await _mediator.Send(
            new ToggleCompleteCommand(id, _currentUserService.UserId ?? "hnjm"));
        
        if (!result.IsSuccess)
        {
            return NotFound(new { errors = result.Errors });
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// إضافة تعليق على مهمة
    /// </summary>
    [HttpPost("{id:int}/comments")]
    [ProducesResponseType(typeof(TodoCommentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoCommentDto>> AddComment(
        int id,
        [FromBody] CreateTodoCommentDto commentDto)
    {
        if (id != commentDto.TodoItemId)
        {
            return BadRequest(new { message = "معرف المهمة غير متطابق" });
        }

        _logger.LogInformation("Add comment to todo {TodoId} by {User}",
            id, _currentUserService.UserName);

        var result = await _mediator.Send(
            new AddCommentCommand(commentDto, _currentUserService.UserId ?? "hnjm"));
        
        if (!result.IsSuccess)
        {
            return BadRequest(new { errors = result.Errors });
        }

        return Created(string.Empty, result.Data);
    }
}