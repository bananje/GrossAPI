using System.ComponentModel.DataAnnotations;

namespace GrossAPI.Models.DTOModel
{
    public class PostDTO
    {
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? ShortDescription { get; set; }
        public string? Header { get; set; }

    }
}
