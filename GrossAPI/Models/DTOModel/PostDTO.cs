using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class PostDTO
    {
        [Required]
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string ShortDescription { get; set; }
        [Required]
        public string Header { get; set; }
    }
}
