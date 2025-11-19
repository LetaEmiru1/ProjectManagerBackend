using Microsoft.AspNetCore.Mvc;

// 1. "ApiController" tells .NET this class handles Web Requests
[ApiController]
// 2. "Route" is the address. This means the URL will be: http://localhost:5042/api/projects
[Route("api/[controller]")] 
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;

    // 3. Dependency Injection: We ask for the "Librarian" (ProjectService) here.
    // The web server gives it to us automatically because we registered it in Program.cs!
    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    // 4. This handles GET requests (like viewing a web page)
    // URL: GET /api/projects/{id}
    [HttpGet("{id}")] 
    public IActionResult GetProject(int id)
    {
        var project = _projectService.GetProjectById(id);
        
        if (project == null)
        {
            return NotFound(); // Returns a 404 Error (like Console.WriteLine("Error"))
        }

        return Ok(project); // Returns a 200 OK with the project data (JSON)
    }

    // 5. This handles POST requests (creating data)
    // URL: POST /api/projects
    [HttpPost]
    public IActionResult CreateProject([FromBody] string name)
    {
        // [FromBody] means "Look inside the data sent with the request to find the name"
        var newProject = _projectService.AddProject(name);
        
        // Returns 200 OK with the new project info
        return Ok(newProject); 
    }
    
    [HttpPost("{projectId}/tasks")]
    public IActionResult AddTask(int projectId, [FromBody] string description)
    {
        var task = _projectService.AddTaskToProject(projectId, description);
        
        if (task != null)
        {
            return Ok(task);
        }
        else
        {
            return NotFound();
        }

    }


}