using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models
{
    public class Categories
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
