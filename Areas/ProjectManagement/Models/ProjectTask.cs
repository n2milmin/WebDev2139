using System.ComponentModel.DataAnnotations;

namespace Lab2.Areas.ProjectManagement.Models
{
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskId { get; set; }
        [Required]
        [Display(Name = "Title")]
		[StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
		public string? Title { get; set; }
        [Required]
		[Display(Name = "Description")]
		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		[DataType(DataType.MultilineText)]
		public string? Description { get; set; }
        // Foreign key
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
