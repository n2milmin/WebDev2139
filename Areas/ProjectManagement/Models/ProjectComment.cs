using System.ComponentModel.DataAnnotations;

namespace Lab2.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int ProjectCommentId { get; set; }

        [Required]
        [Display(Name = "Comment")]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date Posted")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime DatePosted { get; set; }

        // Foreign key for Project
        public int ProjectId { get; set; }

        // Navigation property to Project
        public Project? Project { get; set; }
    }
}
