using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
        
        // Navigation property: allows us to navigate from a Comment to the Stock it belongs to.
        public Stock? Stock { get; set; }
    }
}