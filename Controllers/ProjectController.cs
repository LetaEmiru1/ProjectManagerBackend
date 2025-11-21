using Microsoft.AspNetCore.Mvc;
using ProjectManagerApi; // Needed to see ProjectService

namespace ProjectManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    // 1. GET: api/projects/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        
        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    // 2. POST: api/projects
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] string name)
    {
        var newProject = await _projectService.AddProjectAsync(name);
        return Ok(newProject);
    }

    // 3. POST: api/projects/{projectId}/tasks
    [HttpPost("{projectId}/tasks")]
    public async Task<IActionResult> AddTask(int projectId, [FromBody] string description)
    {
        var task = await _projectService.AddTaskToProjectAsync(projectId, description);
        
        if (task != null)
        {
            return Ok(task);
        }
        
        return NotFound();
    }

    // 4. PUT: api/projects/{projectId}/tasks/{taskId}
    // This exposes your "Mark as Complete" logic to the web!
    [HttpPut("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> MarkTaskComplete(int projectId, int taskId)
    {
        var success = await _projectService.MarkTaskAsCompleteAsync(projectId, taskId);

        if (success)
        {
            return NoContent(); // 204 means "Success, but I have no data to send back"
        }

        return NotFound();
    }
}