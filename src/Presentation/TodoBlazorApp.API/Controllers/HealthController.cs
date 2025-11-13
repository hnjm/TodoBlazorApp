using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBlazorApp.Infrastructure.Data;

namespace TodoBlazorApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(ApplicationDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// التحقق من صحة النظام
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthCheckResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<HealthCheckResponse>> Check()
    {
        var response = new HealthCheckResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = GetType().Assembly.GetName().Version?.ToString() ?? "1.0.0"
        };

        try
        {
            // Check database connection
            var canConnect = await _context.Database.CanConnectAsync();
            response.Database = canConnect ? "Connected" : "Disconnected";

            if (!canConnect)
            {
                response.Status = "Unhealthy";
                return StatusCode(StatusCodes.Status503ServiceUnavailable, response);
            }

            // Get database stats
            response.TodoCount = await _context.TodoItems.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            response.Status = "Unhealthy";
            response.Database = "Error: " + ex.Message;
            return StatusCode(StatusCodes.Status503ServiceUnavailable, response);
        }

        return Ok(response);
    }
}

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public int TodoCount { get; set; }
}