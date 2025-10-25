using backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TasksController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {   Console.WriteLine("GET /api/tasks called");
            // return await _db.Tasks.ToListAsync();
             var tasksFromDb = _db.Tasks.ToList();
    if (tasksFromDb.Any())
    {
        return tasksFromDb;
    }

    // DB is empty → return some dummy tasks
    var dummyTasks = new List<TaskItem>
    {
        new TaskItem { Id = Guid.NewGuid(), Description = "This is a test task", IsCompleted = false },
        new TaskItem { Id = Guid.NewGuid(), Description = "Another test task", IsCompleted = true },
    };

    Console.WriteLine("Returning dummy tasks");
    return dummyTasks;
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(Guid id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return NotFound();
            return task;
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, TaskItem updatedTask)
        {
            if (id != updatedTask.Id) return BadRequest();

            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            // task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
