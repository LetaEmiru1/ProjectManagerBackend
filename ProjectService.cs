using System.Reflection.Metadata.Ecma335;

public class ProjectService
{
    private readonly List<Project> _projects = new List<Project>();
    private int _nextProjectId = 1;
    private int _nextTaskId = 1;

    public Project AddProject(string name)
    {
        var project = new Project { Id = _nextProjectId++, Name = name };
        _projects.Add(project);
        return project;
    }

    public Project? GetProjectById(int id)
    {
        return _projects.FirstOrDefault(p => p.Id == id);
    }

    public TaskItem? AddTaskToProject(int projectId, string taskDescription)
    {
        var project = GetProjectById(projectId);
        if (project == null)
        {
            return null; // Project not found
        }

        var task = new TaskItem { Id = _nextTaskId++, Description = taskDescription, IsCompleted = false };
        project.Tasks.Add(task);
        return task;
    }
    
    public bool MarkTaskAsComplete(int projectId, int taskId)
    {
        var project = GetProjectById(projectId);
        if(project == null)
        {
            return false;
        }
        var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
        
        if(task == null)
        {
            return false;
        }
        task.IsCompleted = true;

        return true;
    }
}