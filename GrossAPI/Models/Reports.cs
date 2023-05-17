using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class Reports
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Description { get; set; }

        public string CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
