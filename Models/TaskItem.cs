namespace backend.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }          // Unique ID for each task
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
