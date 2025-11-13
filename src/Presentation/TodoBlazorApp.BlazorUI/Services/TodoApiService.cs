using System.Net.Http.Json;
using TodoBlazorApp.Application.DTOs;

namespace TodoBlazorApp.BlazorUI.Services;

public interface ITodoApiService
{
    Task<IEnumerable<TodoItemDto>> GetAllAsync(bool includeDetails = false);
    Task<TodoItemDto?> GetByIdAsync(int id, bool includeDetails = true);
    Task<TodoStatsDto> GetStatsAsync();
    Task<IEnumerable<TodoItemDto>> GetOverdueAsync();
    Task<TodoItemDto> CreateAsync(CreateTodoItemDto createDto);
    Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateDto);
    Task DeleteAsync(int id);
    Task<TodoItemDto> ToggleCompleteAsync(int id);
    Task<TodoCommentDto> AddCommentAsync(int todoId, CreateTodoCommentDto commentDto);
    Task<HealthCheckResponse> CheckHealthAsync();
}

public class TodoApiService : ITodoApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TodoApiService> _logger;
    private const string BaseUrl = "api/todos";

    public TodoApiService(HttpClient httpClient, ILogger<TodoApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoItemDto>> GetAllAsync(bool includeDetails = false)
    {
        try
        {
            var url = $"{BaseUrl}?includeDetails={includeDetails}";
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TodoItemDto>>(url);
            return response ?? Enumerable.Empty<TodoItemDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all todos");
            throw;
        }
    }

    public async Task<TodoItemDto?> GetByIdAsync(int id, bool includeDetails = true)
    {
        try
        {
            var url = $"{BaseUrl}/{id}?includeDetails={includeDetails}";
            return await _httpClient.GetFromJsonAsync<TodoItemDto>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching todo {TodoId}", id);
            throw;
        }
    }

    public async Task<TodoStatsDto> GetStatsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TodoStatsDto>($"{BaseUrl}/stats")
                ?? new TodoStatsDto();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching stats");
            throw;
        }
    }

    public async Task<IEnumerable<TodoItemDto>> GetOverdueAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<TodoItemDto>>($"{BaseUrl}/overdue");
            return response ?? Enumerable.Empty<TodoItemDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching overdue todos");
            throw;
        }
    }

    public async Task<TodoItemDto> CreateAsync(CreateTodoItemDto createDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItemDto>()
                ?? throw new Exception("Failed to create todo");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating todo");
            throw;
        }
    }

    public async Task<TodoItemDto> UpdateAsync(int id, UpdateTodoItemDto updateDto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItemDto>()
                ?? throw new Exception("Failed to update todo");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating todo {TodoId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting todo {TodoId}", id);
            throw;
        }
    }

    public async Task<TodoItemDto> ToggleCompleteAsync(int id)
    {
        try
        {
            var response = await _httpClient.PatchAsync($"{BaseUrl}/{id}/toggle", null);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoItemDto>()
                ?? throw new Exception("Failed to toggle todo");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling todo {TodoId}", id);
            throw;
        }
    }

    public async Task<TodoCommentDto> AddCommentAsync(int todoId, CreateTodoCommentDto commentDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/{todoId}/comments", commentDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TodoCommentDto>()
                ?? throw new Exception("Failed to add comment");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding comment to todo {TodoId}", todoId);
            throw;
        }
    }

    public async Task<HealthCheckResponse> CheckHealthAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<HealthCheckResponse>("api/health")
                ?? new HealthCheckResponse { Status = "Unknown" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health");
            return new HealthCheckResponse
            {
                Status = "Unhealthy",
                Database = "Error: " + ex.Message
            };
        }
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