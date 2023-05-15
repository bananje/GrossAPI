using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrossAPI.Models
{
    public class Services
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(1, int.MaxValue)]
        public decimal Price { get; set; }
        public Guid CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Categories Categories { get; set; }
    }
}
