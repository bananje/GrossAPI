using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class CategoryDTO
    {
        [Required]
        public string Title { get; set; }
    }
}
