using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
        [MaxLength(250, ErrorMessage = "Title must be at most 250 characters long")]
        public string Title { get; set; } = string.Empty;
       
        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
        [MaxLength(250, ErrorMessage = "Content must be at most 250 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}