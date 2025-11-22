using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore; // Needed for ToListAsync, FirstOrDefaultAsync
using ProjectManagerApi.Data;

namespace ProjectManagerApi;

public class ProjectService
{
    // 1. We no longer have a List<Project>. We have the DbContext.
    private readonly AppDbContext _context;

    // 2. We ask for the DbContext in the constructor
    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    // 3. Note the "async" and "Task<Project>". This is the Async promise.
    public async Task<Project> AddProjectAsync(string name)
    {
        var project = new Project { Name = name };
        
        // Add to the database tracking (not saved yet)
        _context.Projects.Add(project);
        
        // SAVE CHANGES. This generates the SQL INSERT and runs it.
        // "await" means "pause here until the database is done".
        await _context.SaveChangesAsync();
        
        return project;
    }

    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        // We use Entity Framework to query the DB
        // Include(p => p.Tasks) means "Also load the tasks associated with this project"
        return await _context.Projects
                             .Include(p => p.Tasks)
                             .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<TaskItem?> AddTaskToProjectAsync(int projectId, string description)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null) return null;

        var task = new TaskItem { Description = description, IsCompleted = false };
        
        project.Tasks.Add(task);
        
        // EF Core is smart. It sees we added a task to a project, 
        // so it knows to INSERT the task into the Tasks table.
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<bool> MarkTaskAsCompleteAsync(int projectId, int taskId)
    {
        // We can query the Tasks table directly!
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

        if (task == null) return false;

        task.IsCompleted = true;   
        // This generates an SQL UPDATE command
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return false;
        }
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Project?> UpdateProjectAsync(int id, string newName)
    {
        var project = await _context.Projects.Include(p=>p.Tasks).FirstOrDefaultAsync(p => p.Id ==id);
        if (project == null)
        {
            return null;
        }
        project.Name = newName;
        await _context.SaveChangesAsync();

        return project;
    }
}