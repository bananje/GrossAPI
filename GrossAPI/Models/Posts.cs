using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class Posts
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortDescription { get; set; }
        [Required]
        public string Header { get; set;}
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
