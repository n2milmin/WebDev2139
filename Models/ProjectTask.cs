using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }
        // Foreign key
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
